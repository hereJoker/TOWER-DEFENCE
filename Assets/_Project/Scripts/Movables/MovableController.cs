using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Movables
{
    public sealed class MovableController : IUpdatable
    {
        private readonly IPath _path;
        private readonly List<MovableData> _list = new List<MovableData>();
        private readonly List<IMoveControllerListener> _listeners = new List<IMoveControllerListener>();

        public MovableController(IPath path)
        {
            _path = path;
        }

        public void Register(IMoveControllerListener listener) => _listeners.Add(listener);
        public void Remove(IMoveControllerListener listener) => _listeners.Remove(listener);

        public void RegisterMovable(IMovable movable)
        {
            movable.Position = _path.GetPositionFromTime(0);
            _list.Add(new MovableData
            {
                Target = movable,
                NeedToRemove = false
            });
        }

        public void RemoveMovable(IMovable movable)
        {
            _list.RemoveAll(_ => _.Target == movable);
        }

        public void Update(in float deltaTime)
        {
            _list.RemoveAll(_ => _.NeedToRemove || _.Target.IsOnPath == false);

            foreach (var data in _list)
            {
                var time = _path.GetTime(data.Target.Position);
                if (time >= 1 - deltaTime - float.Epsilon)
                {
                    data.Target.Position = _path.GetPositionFromTime(1f);
                    data.NeedToRemove = true;
                    foreach (var listener in _listeners)
                    {
                        listener.OnFinishMovable(data.Target);
                    }
                    data.Target.OnFinishPath();
                    continue;
                }

                var nexTime = time + deltaTime;
                var nextPos = _path.GetPositionFromTime(nexTime);
                var dir = (nextPos - data.Target.Position).normalized;
                data.Target.Position +=
                    Vector3.Lerp(Vector3.zero, dir * (data.Target.Speed * deltaTime), data.Target.SlowDownFactor);
                data.Target.Rotation = Quaternion.LookRotation(dir);
            }
        }
    }


    internal sealed class MovableData
    {
        public IMovable Target;
        public bool NeedToRemove;
    }

    public interface IPath
    {
        public Vector3 GetPositionFromTime(in float time);
        public float GetTime(Vector3 position);
    }

    public interface IMoveControllerListener
    {
        void OnFinishMovable(IMovable movable);
    }

    public interface IMovable
    {
        bool IsOnPath { get; }

        public float SlowDownFactor { get; }

        public float Speed { get; }

        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }

        void OnFinishPath();
    }
}