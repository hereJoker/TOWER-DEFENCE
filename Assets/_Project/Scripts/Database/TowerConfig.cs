using _Project.Scripts.Tower;
using _Project.Scripts.Unity;
using UnityEngine;

namespace _Project.Scripts.Database
{
    [CreateAssetMenu(fileName = "TowerConfig", menuName = "_Config/TowerConfig", order = 1)]
    public sealed class TowerConfig : ScriptableObject
    {
        public TowerItem[] Items;
    }

    [System.Serializable]
    public sealed class TowerItem
    {
        public TowerStats Stats;
        public TowerType Type;
        public TowerView Tower;
        public Sprite Preview;
    }

    [System.Serializable]
    public enum TowerType
    {
        TUltraDamage, TDamage, TSlow, TRange, TSpeed
    }
}
