using _Project.Scripts.Unity;
using UnityEngine;

namespace _Project.Scripts.Database
{
    [CreateAssetMenu(fileName = "UnitConfig", menuName = "_Config/UnitConfig", order = 1)]
    public sealed class UnitConfig : ScriptableObject
    {
        public UnitItem[] Items;
    }

    [System.Serializable]
    public sealed class UnitItem
    {
        public UnitView Prefab;
        public float Hp;
        public float Speed = 10;
    }
}
