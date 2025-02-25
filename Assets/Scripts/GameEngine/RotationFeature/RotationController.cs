using Application.GameCycleFeature;
using GameEngine.DayFeature;

namespace GameEngine.RotationFeature
{
    public sealed class RotationController : IInitializable, IFinishable
    { 
        private readonly GameCycleManager _gameCycle;
        private readonly Day _day;
        private readonly RotationComponent _sun;
        private readonly RotationComponent _moon;

        public RotationController(GameCycleManager gameCycle, Day day, RotationComponent sun, RotationComponent moon)
        {
            _gameCycle = gameCycle;
            _day = day;
            _sun = sun;
            _moon = moon;
        }

        void IInitializable.OnInitialize()
        {
            _day.OnDayStarted += RotateSun;
            _day.OnNightStarted += RotateMoon;
        }

        void IFinishable.OnFinish()
        {
            _day.OnDayStarted -= RotateSun;
            _day.OnNightStarted -= RotateMoon;
        }

        private void RotateSun(DayTime _)
        {
            _moon.ResetRotation();
            
            _gameCycle.AddListener(_sun);
            _gameCycle.RemoveListener(_moon);
        }

        private void RotateMoon(DayTime _)
        {
            _sun.ResetRotation();
            
            _gameCycle.AddListener(_moon);
            _gameCycle.RemoveListener(_sun);
        }
    }
}