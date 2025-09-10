using System.Collections.Generic;

namespace _Project.Scripts.Tower
{
    public sealed class SlowAoeTower : AoeTower
    {
        public SlowAoeTower(TowerStats stats, ITowerView view) : base(stats, view)
        {
        }

        protected override void OnPerformOnUnits(List<IDamageable> list)
        {
            _view.OnSlowUnits(list[0].Position, list.ToArray());

            foreach (var damageable in list)
            {
                damageable.SlowDown(_stats.SlowTime);
                damageable.TakeDamage(_stats.Damage);
            }
        }
    }
}