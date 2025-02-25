using System;
using UnityEngine;
using Common;
using Application.GameCycleFeature;

namespace GameEngine.DayFeature
{
    public sealed class Day : IStartable, IUpdatable
    {
        internal event Action<float> OnElapsedTimeChanged;
        public event Action<DayTime> OnDayStarted;
        public event Action<DayTime> OnNightStarted;

        private DayTime _dayTime;

        private readonly Countdown _countdown;
        private readonly int _nightStartTime;

        public Day(Countdown countdown, int nightStartTime)
        {
            _countdown = countdown;
            _nightStartTime = nightStartTime;
        }

        void IStartable.OnStart()
        {
            OnDayStarted?.Invoke(_dayTime);
        }

        void IUpdatable.OnUpdate()
        {
            if (_countdown.IsPlaying())
            {
                float elapsedTime = _countdown.ElapsedTime;
                _countdown.Tick(Time.deltaTime);
                
                if (_dayTime == DayTime.Day && elapsedTime >= _nightStartTime)
                {
                    _dayTime = DayTime.Night;
                    OnNightStarted?.Invoke(_dayTime);
                }

                OnElapsedTimeChanged?.Invoke(elapsedTime);
                return;
            }

            _dayTime = DayTime.Day;
            OnDayStarted?.Invoke(_dayTime);

            _countdown.Reset();
        }
    }
}