using System.Collections.Generic;
using Application.GameCycleFeature;
using UnityEngine;

namespace GameEngine.DayFeature
{
    public sealed class DayController : IInitializable, IFinishable
    {
        private DayTime _currentDayTime;
        
        private readonly Day _day;
        private readonly DayLightSwitcher _lightSwitcher;
        private readonly IReadOnlyDictionary<DayTime, Light> _lights;

        public DayController(Day day, DayLightSwitcher lightSwitcher, IReadOnlyDictionary<DayTime, Light> lights)
        {
            _day = day;
            _lightSwitcher = lightSwitcher;
            _lights = lights;
        }

        void IInitializable.OnInitialize()
        {
            _day.OnDayStarted += OnDayLight;
            _day.OnNightStarted += OnNightLight;
            _day.OnElapsedTimeChanged += SwitchSpaceColour;
        }

        void IFinishable.OnFinish()
        {
            _day.OnDayStarted -= OnDayLight;
            _day.OnNightStarted -= OnNightLight;
            _day.OnElapsedTimeChanged -= SwitchSpaceColour;
        }

        private void OnDayLight(DayTime dayTime)
        {
            _currentDayTime = dayTime;
            _lightSwitcher.OnDayLight(_lights[dayTime]);
        }

        private void OnNightLight(DayTime dayTime)
        {
            _currentDayTime = dayTime;
            _lightSwitcher.OnNightLight(_lights[dayTime]);
        }

        private void SwitchSpaceColour(float elapsedTime)
        {
            _lightSwitcher.SwitchSpaceColour(elapsedTime, _lights[_currentDayTime]);
        }
    }
}