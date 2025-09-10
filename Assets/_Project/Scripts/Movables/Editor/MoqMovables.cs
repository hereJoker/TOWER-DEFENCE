using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Movables.Editor
{

    internal class DummyMovable : IMovable
    {
        public bool IsFinish;
        public bool IsOnPath { get; set; } = true;
        public float SlowDownFactor { get; set; } = 1;
        public float Speed { get; set; } = 1;
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public void OnFinishPath()
        {
            IsFinish = true;
        }
    }

    internal class DummyPath : IPath
    {
        public Vector3 GetPositionFromTime(in float time)
        {
            return Vector3.forward * time;
        }

        public float GetTime(Vector3 position)
        {
            return position.z;
        }
    }

    internal class DummyListener : IMoveControllerListener
    {
        public List<IMovable> Movable = new List<IMovable>();
        public void OnFinishMovable(IMovable movable)
        {
            Movable.Add(movable);
        }
    }
}