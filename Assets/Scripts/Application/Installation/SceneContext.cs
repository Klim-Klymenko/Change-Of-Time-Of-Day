using System;
using System.Linq;
using UnityEngine;
using Application.GameCycleFeature;
using Common;
using GameEngine.DayFeature;
using GameEngine.RotationFeature;

namespace Application.Installation
{
    [DefaultExecutionOrder(-5000)]
    internal sealed class SceneContext : MonoBehaviour
    {
        [Header("Day")]
        
        [SerializeField] 
        private string _startingTime = "6:00";
        
        [SerializeField] 
        private int _dayLength = 120;
        
        [SerializeField]
        private ColourData[] _dayData;

        [SerializeField]
        private ColourData[] _nightData;

        [SerializeField] 
        private GameObject[] _dayLight;
        
        [SerializeField] 
        private GameObject[] _nightLight;
        
        [SerializeField]
        private LightningData[] _lightningData;

        [Header("Rotation")] 
        
        [SerializeField]
        private RotationComponent _sun;
        
        [SerializeField]
        private RotationComponent _moon;
        
        private GameCycleManager _gameCycle;
        
        private void Awake()
        {
            _gameCycle = new GameCycleManager();
            
            Day day = InstallDay();
            InstallRotation(day);
            
            _gameCycle.OnInitialize();
        }
        
        private void Start()
        {
            _gameCycle.OnStart();
        }

        private void Update()
        {
            _gameCycle.OnUpdate();
        }

        private void OnDestroy()
        {
            _gameCycle.OnFinish();
        }
        
        private Day InstallDay()
        {
            int startingTime = ParseTime(_startingTime);
            Countdown countdown = new(_dayLength, startingTime);

            int nightStartTime = ParseTime(_nightData[0].Time);
            Day day = new(countdown, nightStartTime);

            int dayColoursCount = _dayData.Length;
            int totalColoursCount = dayColoursCount + _nightData.Length;
            
            Color[] colours = new Color[totalColoursCount];
            int[] ranges = new int[totalColoursCount];
            
            for (int i = 0; i < totalColoursCount; i++)
            {
                Color colour = i < dayColoursCount ? _dayData[i].Colour : _nightData[i - dayColoursCount].Colour;
                
                int range;
                
                if (i < dayColoursCount)
                    range = ParseTime(_dayData[i].Time);
                else
                {
                    range = ParseTime(_nightData[i - dayColoursCount].Time);
                    
                    if (range < nightStartTime)
                        range += _dayLength;
                }

                colours[i] = colour;
                ranges[i] = range;
            }

            const float dayLengthInLerpUnit = 120.0f;
            const float lerpUnit = 0.1f;
            float lerpSize = dayLengthInLerpUnit / _dayLength;
            float lerpSpeed = lerpUnit * lerpSize;
            
            DayLightSwitcher lightSwitcher = new(_dayLight, _nightLight, colours, _dayData[0].Colour, _nightData[0].Colour, ranges, lerpSpeed, _dayLength);
            DayController dayController = new(day, lightSwitcher, _lightningData.ToDictionary(x => x.DayTime, x => x.Light));
            
            _gameCycle.AddListener(day);
            _gameCycle.AddListener(dayController);

            return day;
        }

        private void InstallRotation(Day day)
        {
            RotationController rotationController = new(_gameCycle, day, _sun, _moon);
            
            const float degreesToRotate = 360.0f;
            const float dayMultiplier = 1 - 1.0f / 25.0f;
            const float nightDivider = 1 - 1.0f / 16.0f;
            
            float dayLength =  _dayLength / ((float) _dayData.Length / _nightData.Length);
            float nightLength = _dayLength - dayLength;
            
            float sunSpeed = degreesToRotate / dayLength * dayMultiplier;
            float moonSpeed = degreesToRotate / nightLength / nightDivider;
            
            _sun.Construct(sunSpeed);
            _moon.Construct(moonSpeed);
            
            _gameCycle.AddListener(rotationController);
            _gameCycle.AddListener(_sun);
            _gameCycle.AddListener(_moon);
        }
        
        private int ParseTime(string time)
        {
            const int secondsInDay = 24 * 60 * 60;
            int gameTimeToReal = secondsInDay / _dayLength;

            TimeSpan timeSpan = TimeSpan.Parse(time);
            int realSeconds = (int) timeSpan.TotalSeconds;

            return realSeconds / gameTimeToReal;
        }
    }
}