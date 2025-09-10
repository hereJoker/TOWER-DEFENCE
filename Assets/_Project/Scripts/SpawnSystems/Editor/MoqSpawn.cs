using System.Collections.Generic;

namespace _Project.Scripts.SpawnSystems.Editor
{
    internal class DummyFabric : IUnitFabric
    {
        public List<DummyUnit> Units = new List<DummyUnit>();
        private bool _isAliveAtStart;

        public DummyFabric(bool isAliveAtStart = false)
        {
            _isAliveAtStart = isAliveAtStart;
        }


        public IUnit CreateUnit(int type)
        {
            var u = new DummyUnit();
            u.IsAlive = _isAliveAtStart;
            Units.Add(u);
            return u;
        }

        public void DestroyUnit(int type, IUnit obj)
        {
            Units.Remove((DummyUnit)obj);
        }
    }

    internal class DummyUnit : IUnit
    {
        public bool IsAlive { get; set; } = false;
        public bool IsOnPath { get; set; }
    }

    internal class DummyUnitListener : IWaveUnitListener
    {
        public IUnit Unit;
        public void OnCreateUnit(IUnit unit)
        {
            Unit = unit;
        }
    }


}