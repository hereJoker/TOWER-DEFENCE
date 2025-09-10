using System.Threading;
using _Project.Scripts.Movables;
using _Project.Scripts.SpawnSystems;

namespace _Project.Scripts.Gameplay
{
    public sealed class RouterWaveToMovable : IWaveUnitListener
    {
        private MovableController _movable;
        private WaveController _wave;

        public void Init(MovableController movable)
        {
            _movable = movable;
        }

        public void OnCreateUnit(IUnit unit)
        {
            var move = (IMovable)unit;
            if (move != null)
            {
                _movable.RegisterMovable(move);
            }
        }
    }
}