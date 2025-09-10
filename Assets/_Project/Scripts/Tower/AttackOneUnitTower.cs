namespace _Project.Scripts.Tower
{
    public sealed class AttackOneUnitTower : OneUnitTower
    {
        public AttackOneUnitTower(TowerStats stats, ITowerView view) : base(stats, view)
        {
        }

        protected override void OnPerformOnUnit(IDamageable damageable)
        {
            damageable.TakeDamage(_stats.Damage);
            _view.OnAttackUnit(damageable);
        }
    }
}