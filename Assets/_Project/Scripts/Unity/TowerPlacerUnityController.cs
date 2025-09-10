using System;
using _Project.Scripts.Database;
using _Project.Scripts.ObjectPlacer;
using _Project.Scripts.Tower;
using _Project.Scripts.Unity.Effects;
using _Project.Scripts.Unity.UI;
using UnityEngine;

namespace _Project.Scripts.Unity
{
    public sealed class TowerPlacerUnityController : ISelectTowerItem, IUpdatable
    {
        private readonly Camera _camera;
        private readonly ObjectBoundsPlacer _placer;
        private readonly UnityEffects _effects;
        private bool _isSelected;
        private readonly LayerMask _mask;
        private TowerView _view;
        private TowerItem _item;

        public int AmountOfTowerPlaced { get; private set; } = 0;

        public TowerPlacerUnityController(ObjectBoundsPlacer boundsPlacer, Camera camera, LayerMask mask, UnityEffects effects)
        {
            _mask = mask;
            _effects = effects;
            _placer = boundsPlacer;
            _camera = camera;
        }
        
        public void OnSelectTower(TowerItem item)
        {
            _item = item;
            _isSelected = true;
            _view = GameObject.Instantiate(item.Tower);
            _effects.OnAddTower(_view.Position, _item.Stats.DamageDistance);
        }

        public void Update(in float deltaTime)
        {
            if (_isSelected == false)
                return;

            if (Input.GetMouseButtonUp(0))
            {
                SetPlace();
                return;
            }

            if (Input.GetMouseButton(0))
            {
                MoveTower();
            }
        }

        private void ResetView()
        {
            _effects.RemoveLast();
            GameObject.Destroy(_view.gameObject);
            _view = null;
            _item = null;
        }

        private void SetPlace()
        {
            _isSelected = false;
            if (_placer.Place(_view))
            {
                AmountOfTowerPlaced++;
                _view.SetPlace();
                _effects.UpdateLastTower(_view.Position, _item.Stats.DamageDistance);
                switch (_item.Type)
                {
                    case TowerType.TUltraDamage:
                        _view.Init(new AttackOneUnitTower(_item.Stats, _view), _effects);
                        break;
                    case TowerType.TDamage:
                        _view.Init(new AttackAoeTower(_item.Stats, _view), _effects);
                        break;
                    case TowerType.TSlow:
                        _view.Init(new SlowAoeTower(_item.Stats, _view), _effects);
                        break;
                    case TowerType.TSpeed:
                        _view.Init(new AttackAoeTower(_item.Stats, _view), _effects);
                        break;
                    case TowerType.TRange:
                        _view.Init(new AttackOneUnitTower(_item.Stats, _view), _effects);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                ResetView();
            }
        }

        private void MoveTower()
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var raycastHi, float.MaxValue, _mask))
            {
                _view.transform.position = raycastHi.point;
                _effects.UpdateLastTower(raycastHi.point, _item.Stats.DamageDistance);
                if (_placer.IsAbleToPlace(raycastHi.point))
                {
                    _view.SetPossibleToPlace();
                }
                else
                {
                    _view.SetImpossibleToPlace();
                }
            }
        }
        
    }
}
