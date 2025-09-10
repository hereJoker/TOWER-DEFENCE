using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Tower
{
    public abstract class AoeTower : BaseTower
    {
        private readonly List<IDamageable> _cached = new List<IDamageable>();
        
        protected AoeTower(TowerStats stats, ITowerView view) : base(stats, view)
        {
        }

        protected override void OnPerformAction()
        {
            if (_list.Count == 0)
                return;

            _cached.Clear();
            var p = _list[0].Position;
            foreach (var damageable in _list)
            {
                if (Vector3.Distance(p, damageable.Position) <= _stats.AoeRange)
                {
                    _cached.Add(damageable);
                }
            }
            _view.OnLookAt(p);
            OnPerformOnUnits(_cached);
        }

        protected abstract void OnPerformOnUnits(List<IDamageable> list);



    }
}