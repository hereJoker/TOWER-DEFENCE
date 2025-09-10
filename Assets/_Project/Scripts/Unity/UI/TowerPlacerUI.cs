using System.Collections.Generic;
using _Project.Scripts.Database;
using UnityEngine;

namespace _Project.Scripts.Unity.UI
{
    public sealed class TowerPlacerUI : MonoBehaviour
    {
        [SerializeField] private TowerPlacerView _prefab;
        [SerializeField] private Transform _root;
        [SerializeField] private TowerConfig _config;

        private readonly List<ISelectTowerItem> _list = new List<ISelectTowerItem>();

        private void Awake()
        {
            _root.gameObject.SetActive(true);
            foreach (var item in _config.Items)
            {
                var obj = Instantiate(_prefab, _root);
                obj.Init(item, OnSelect);
            }
        }

        public void Register(ISelectTowerItem item) => _list.Add(item);
        public void Remove(ISelectTowerItem item) => _list.Remove(item);

        private void OnSelect(TowerItem obj)
        {
            foreach (var item in _list)
            {
                item.OnSelectTower(obj);
            }
        }
    }

    public interface ISelectTowerItem
    {
        void OnSelectTower(TowerItem item);
    }
    
}
