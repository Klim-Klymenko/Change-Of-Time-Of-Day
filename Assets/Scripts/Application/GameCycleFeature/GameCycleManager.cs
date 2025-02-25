using System.Collections.Generic;

namespace Application.GameCycleFeature
{
    public sealed class GameCycleManager
    {
        private GameState _gameState;

        private readonly List<IInitializable> _initializableListeners = new();
        private readonly List<IStartable> _startableListeners = new();
        private readonly List<IUpdatable> _updatableListeners = new();
        private readonly List<IFinishable> _finishableListeners = new();

        public void OnInitialize()
        {
            if (_gameState != GameState.None)
                return;

            for (int i = 0; i < _initializableListeners.Count; i++)
            {
                _initializableListeners[i].OnInitialize();
            }

            _gameState = GameState.Initialized;
        }

        public void OnStart()
        {
            if (_gameState != GameState.Initialized)
                return;

            for (int i = 0; i < _startableListeners.Count; i++)
            {
                _startableListeners[i].OnStart();
            }

            _gameState = GameState.Active;
        }

        public void OnUpdate()
        {
            if (_gameState != GameState.Active)
                return;

            for (int i = 0; i < _updatableListeners.Count; i++)
            {
                _updatableListeners[i].OnUpdate();
            }
        }


        public void OnFinish()
        {
            if (_gameState == GameState.Finished)
                return;

            for (int i = 0; i < _finishableListeners.Count; i++)
            {
                _finishableListeners[i].OnFinish();
            }

            _gameState = GameState.Finished;
        }

        public void AddListener(IGameListener listener)
        {
            if (listener is IInitializable initializableListener)
                if (!_initializableListeners.Contains(initializableListener))
                    _initializableListeners.Add(initializableListener);

            if (listener is IStartable startableListener)
                if (!_startableListeners.Contains(startableListener))
                    _startableListeners.Add(startableListener);

            if (listener is IUpdatable updatableListener)
                if (!_updatableListeners.Contains(updatableListener))
                    _updatableListeners.Add(updatableListener);

            if (listener is IFinishable finishable)
                if (!_finishableListeners.Contains(finishable))
                    _finishableListeners.Add(finishable);
        }

        public void RemoveListener(IGameListener listener)
        {
            if (listener is IInitializable initializableListener)
                if (_initializableListeners.Contains(initializableListener))
                    _initializableListeners.Remove(initializableListener);

            if (listener is IStartable startableListener)
                if (_startableListeners.Contains(startableListener))
                    _startableListeners.Remove(startableListener);

            if (listener is IUpdatable updatableListener)
                if (_updatableListeners.Contains(updatableListener))
                    _updatableListeners.Remove(updatableListener);

            if (listener is IFinishable finishableListener)
                if (_finishableListeners.Contains(finishableListener))
                    _finishableListeners.Remove(finishableListener);
        }
    }
}