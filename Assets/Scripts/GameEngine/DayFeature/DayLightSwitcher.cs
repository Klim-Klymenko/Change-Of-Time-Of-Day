using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.DayFeature
{
    public sealed class DayLightSwitcher
    {
        private int _currentIndex;
        private int _currentMaxRange;
        private Color _currentColour;
        private Color _targetColour;

        private readonly IReadOnlyList<GameObject> _dayLight;
        private readonly IReadOnlyList<GameObject> _nightLight;
        private readonly IReadOnlyList<Color> _colours;
        private readonly Color _dayColour;
        private readonly Color _nightColour;
        private readonly IReadOnlyList<int> _ranges;
        private readonly float _colourLerpSpeed;
        private readonly int _dayLength;

        public DayLightSwitcher(IReadOnlyList<GameObject> dayLight, IReadOnlyList<GameObject> nightLight,
            IReadOnlyList<Color> colours, Color dayColour, Color nightColour,
            IReadOnlyList<int> ranges, float colourLerpSpeed, int dayLength)
        {
            _dayLight = dayLight;
            _nightLight = nightLight;
            _colours = colours;
            _dayColour = dayColour;
            _nightColour = nightColour;
            _ranges = ranges;
            _colourLerpSpeed = colourLerpSpeed;
            _dayLength = dayLength;
            
            SetTargetColour();
        }

        internal void SwitchSpaceColour(float elapsedTime, Light light)
        {
            if (elapsedTime > _currentMaxRange)
            {
                _currentIndex++;
                bool hasNextColour = _currentIndex + 1 < _colours.Count;
                
                _targetColour = hasNextColour ? _colours[_currentIndex + 1] : _colours[0];
                _currentMaxRange = hasNextColour ? _ranges[_currentIndex + 1] : _ranges[0] + _dayLength;
            }

            light.color = Color.Lerp(light.color, _targetColour, _colourLerpSpeed * Time.deltaTime);
        }

        internal void OnDayLight(Light light)
        {
            _currentIndex = 0;
            light.color = _dayColour;
            
            SetTargetColour();
            SwitchNightLight(false);
            SwitchDayLight(true);
        }

        internal void OnNightLight(Light light)
        {
            light.color = _nightColour;
            
            SwitchDayLight(false);
            SwitchNightLight(true);
        }

        private void SetTargetColour()
        {
            _targetColour = _colours[_currentIndex + 1];
            _currentMaxRange = _ranges[_currentIndex + 1];
        }
        
        private void SwitchDayLight(bool switchState)
        {
            for (int i = 0; i < _dayLight.Count; i++)
            {
                _dayLight[i].SetActive(switchState);
            }
        }

        private void SwitchNightLight(bool switchState)
        {
            for (int i = 0; i < _nightLight.Count; i++)
            {
                _nightLight[i].SetActive(switchState);
            }
        }
    }
}