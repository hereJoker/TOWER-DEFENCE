using System.Collections.Generic;
using _Project.Scripts.SpawnSystems;
using UnityEngine;

namespace _Project.Scripts.Tower
{
    public abstract class BaseTower : IUpdatable
    {
        protected readonly List<IDamageable> _list = new List<IDamageable>();
        protected readonly TowerStats _stats;
        protected readonly ITowerView _view;

        public float CurrentAttackDistance => _stats.DamageDistance;

        private float _currentTime;

        protected BaseTower(TowerStats stats, ITowerView view)
        {
            _stats = stats;
            _view = view;
            _currentTime = stats.DelayExecution + float.Epsilon; // immediate attack 
        }


        public void Add(IDamageable damageable)
        {
            if (_list.Contains(damageable) == false)
                _list.Add(damageable);
        }

        public void Remove(IDamageable damageable) => _list.Remove(damageable);


        public void Update(in float deltaTime)
        {
            _currentTime += deltaTime;
            _list.RemoveAll(_ => _.IsAlive == false);
            if (_currentTime >= _stats.DelayExecution)
            {
                if (_list.Count > 0)
                {
                    _currentTime = 0;
                }
                else
                {
                    return;
                }

                OnPerformAction();
            }
        }

        protected abstract void OnPerformAction();
    }

    public interface ITowerView
    {
        void OnLookAt(Vector3 pos);

        void OnAttackUnit(IDamageable unit);
        void OnAttackUnits(Vector3 pos, IDamageable[] unit);
        void OnSlowUnit(IDamageable unit);
        void OnSlowUnits(Vector3 pos, IDamageable[] unit);
    }

    [System.Serializable]
    public sealed class TowerStats
    {
        public float Damage;
        public float SlowTime;
        public float DelayExecution;
        public float AoeRange;
        public float DamageDistance;
    }

    public interface IDamageable : IUnit
    {
        void TakeDamage(in float amount);

        void SlowDown(in float time);
        Vector3 Position { get; }
    }
}
