using _Project.Scripts.SpawnSystems;
using UnityEngine;

namespace _Project.Scripts.Database
{
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "_Config/WaveConfig", order = 1)]
    public class WaveConfig : ScriptableObject
    {
        public int TotalLives = 20;
        public WaveSettings WaveSettings;
    }
}
