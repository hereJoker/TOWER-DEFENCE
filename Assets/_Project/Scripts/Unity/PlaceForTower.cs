using System;
using UnityEngine;

namespace _Project.Scripts.Unity
{
    public class PlaceForTower : MonoBehaviour
    {
        [SerializeField] private Collider _collider;

        public Bounds GetBounds()
        {
            return _collider.bounds;
        }

        
        private void OnDrawGizmos()
        {
            var c = Gizmos.color;
            
            Gizmos.color = Color.red;
            var b = GetBounds();
            Gizmos.DrawWireCube(b.center, b.size);

            Gizmos.color = c;
        }
    }
}
