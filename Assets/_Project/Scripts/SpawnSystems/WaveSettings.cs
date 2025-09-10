using System.Collections.Generic;

namespace _Project.Scripts.SpawnSystems
{
    [System.Serializable]
    public sealed class WaveSettings
    {
        public List<Wave> Waves;
    }

    [System.Serializable]
    public sealed class Wave
    {
        public float Delay;
        public int Type;
        public int Amount;
    }

}