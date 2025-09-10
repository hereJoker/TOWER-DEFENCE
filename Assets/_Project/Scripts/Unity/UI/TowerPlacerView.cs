using _Project.Scripts.Database;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Unity.UI
{
    public class TowerPlacerView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private System.Action<TowerItem> _onSelect;
        private TowerItem _item;
                
        public void Init(TowerItem item, System.Action<TowerItem> onSelect)
        {
            _item = item;
            _image.sprite = _item.Preview;
            _onSelect = onSelect;
        }
        
        public void OnSelect()
        {
            if (Input.GetMouseButtonDown(0) == false)
                return;
            
            _onSelect?.Invoke(_item);
        }
    }
}
