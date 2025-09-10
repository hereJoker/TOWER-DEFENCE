using System;
using _Project.Scripts.ObjectPlacer;
using _Project.Scripts.Tower;
using _Project.Scripts.Unity.Effects;
using UnityEngine;

namespace _Project.Scripts.Unity
{
    public sealed class TowerView : MonoBehaviour, IPlaceable, ITowerView
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _startColor1;
        [SerializeField] private Color _startColor2;
        [SerializeField] private SphereCollider _trigger;

        private MaterialPropertyBlock _block;
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static Collider[] _colliders = new Collider[30];
        private BaseTower _tower;

        private UnityEffects _effects;


        public Vector3 Position
        {
            get => _tr.position;
            set => _tr.position = value;
        }

        public Bounds Bounds => _renderer.bounds;

        private Transform _tr;

        private void Awake()
        {
            _tr = transform;
            _block = new MaterialPropertyBlock();
        }

        public void SetImpossibleToPlace()
        {
            SetColors(Color.red);
        }

        public void SetPossibleToPlace()
        {
            SetColors(Color.green);
        }

        public void SetPlace()
        {
            SetColors(_startColor1, _startColor2);
        }

        private void SetColors(Color color0, Color? color1 = null)
        {
            var mats = _renderer.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                var block = new MaterialPropertyBlock();
                block.SetColor(ColorId, i == 0 ? color0 : color1 ?? color0);
                _renderer.SetPropertyBlock(block, i);
            }
        }


        public void Init(BaseTower tower, UnityEffects effects)
        {
            _effects = effects;
            _tower = tower;
            _trigger.radius = _tower.CurrentAttackDistance *2;

            // if it was placed when it is running the wave, at Trigger Enter may not be executed
            var size = Physics.OverlapSphereNonAlloc(_tr.position, _tower.CurrentAttackDistance, _colliders);
            for (int i = 0; i < size; i++)
            {
                OnTriggerEnter(_colliders[i]);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_tower == null)
                return;

            var c = other.GetComponent<IDamageable>();
            if (c != null)
            {
                _tower.Add(c);
            }
        }
        

        private void OnTriggerExit(Collider other)
        {
            if (_tower == null)
                return;
            
            var c = other.GetComponent<IDamageable>();
            if (c != null)
            {
                _tower.Remove(c);
            }
        }

        private void Update()
        {
            if (_tower != null)
                _tower.Update(Time.deltaTime);
        }

        public void OnLookAt(Vector3 pos)
        {
            pos.y = _tr.position.y;
            _tr.LookAt(pos);
        }

        public void OnAttackUnit(IDamageable unit)
        {
            // because it's engine specific, we can cast
            var unityView = (UnitView)unit;
            if (unityView != null)
            {
                _effects.ProjectileAttack(_tr.position, unityView.transform, () =>
                {
                    unityView.UpdateView();
                });
            }
        }

        public void OnAttackUnits(Vector3 pos, IDamageable[] units)
        {
            _effects.ProjectileAoeAttack(_tr.position, pos, () =>
            {
                foreach (var unit in units)
                {
                    // because it's engine specific, we can cast
                    var unityView = (UnitView)unit;
                    if (unityView != null)
                    {
                        unityView.UpdateView();
                    }
                }
            });
        }
        

        public void OnSlowUnit(IDamageable unit)
        {
            // because it's engine specific, we can cast
            var unityView = (UnitView)unit;
            if (unityView != null)
            {
                _effects.ProjectileSlow(_tr.position, unityView.transform, () =>
                {
                    unityView.UpdateView();
                });
            }
        }

        public void OnSlowUnits(Vector3 pos, IDamageable[] units)
        {
            _effects.ProjectileAoeSlow(_tr.position, pos, () =>
            {
                foreach (var unit in units)
                {
                    // because it's engine specific, we can cast
                    var unityView = (UnitView)unit;
                    if (unityView != null)
                    {
                        unityView.UpdateView();
                    }
                }
            });
        }

    }
}
