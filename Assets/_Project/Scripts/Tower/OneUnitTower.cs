namespace _Project.Scripts.Tower
{
    public abstract class OneUnitTower : BaseTower
    {
        public OneUnitTower(TowerStats stats, ITowerView view) : base(stats, view)
        {
        }

        protected override void OnPerformAction()
        {
            if (_list.Count == 0)
                return;
            
            _view.OnLookAt(_list[0].Position);
            OnPerformOnUnit(_list[0]);
        }
        protected abstract void OnPerformOnUnit(IDamageable damageable);
    }
}