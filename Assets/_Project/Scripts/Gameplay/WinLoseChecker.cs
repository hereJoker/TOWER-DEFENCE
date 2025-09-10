using System.Collections.Generic;
using _Project.Scripts.Movables;
using _Project.Scripts.SpawnSystems;

namespace _Project.Scripts.Gameplay
{
    public sealed class WinLoseChecker : IMoveControllerListener, IWaveStateListener
    {
        public int TotalLives { get; private set; }

        private readonly List<IWinLoseListener> _listeners = new List<IWinLoseListener>();

        public void RegisterListener(IWinLoseListener l) => _listeners.Add(l);
        public void RemoveListener(IWinLoseListener l) => _listeners.Remove(l);

        public WinLoseChecker(int totalLives)
        {
            TotalLives = totalLives;
        }

        public void OnFinishMovable(IMovable movable)
        {
            TotalLives--;
            foreach (var listener in _listeners)
            {
                listener.OnUpdateLives(TotalLives);
            }
            
            if (TotalLives <= 0)
            {
                foreach (var listener in _listeners)
                {
                    listener.OnLose();
                }
            }
        }

        public void OnStartWave(int index)
        {
            
        }

        public void OnFinishWave(int index)
        {
        }

        public void OnFinishAllWaves()
        {
            foreach (var listener in _listeners)
            {
                listener.OnWin();
            }
        }
    }

    public interface IWinLoseListener
    {
        void OnWin();
        void OnLose();
        void OnUpdateLives(in int lives);
    }
    
}