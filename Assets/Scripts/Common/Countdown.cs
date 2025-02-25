namespace Common
{
    public sealed class Countdown
    {
        public float ElapsedTime => _currentTime;

        private const byte StartTime = 0;
        private float _currentTime;
        
        private readonly float _delay;
        private readonly float _offset;

        public Countdown(float delay)
        {
            _delay = delay;
            _currentTime = delay;
        }
        
        public Countdown(float delay, int offset)
        {
            _offset = offset;
            _delay = delay + offset;
            _currentTime = offset;
        }
        public void Tick(float deltaTime)
        {
            _currentTime += deltaTime;
        }

        public void Reset()
        {
            _currentTime = StartTime + _offset;
        }

        public bool IsPlaying()
        {
            return _currentTime < _delay;
        }

        public bool IsFinished()
        {
            return _currentTime >= _delay;
        }
    }
}