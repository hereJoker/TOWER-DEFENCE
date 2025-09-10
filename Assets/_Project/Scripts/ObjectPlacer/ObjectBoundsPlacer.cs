using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.ObjectPlacer
{
    public sealed class ObjectBoundsPlacer
    {
        private Bounds _bounds;
        public Bounds Bounds => _bounds;
        private readonly List<IPlaceable> _list = new List<IPlaceable>();

        public ObjectBoundsPlacer(Bounds bounds)
        {
            _bounds = bounds;
        }

        public bool IsAbleToPlace(Vector3 pos)
        {
            if (_bounds.Contains(pos) == false)
                return false;

            foreach (var placer in _list)
            {
                if (placer.Bounds.Contains(pos))
                    return false;
            }

            return true;
        }

        public bool Place(IPlaceable obj)
        {
            if (IsAbleToPlace(obj.Position) == false)
                return false;
            _list.Add(obj);
            return true;
        }
    }

    public interface IPlaceable
    {
        Vector3 Position { get; set; }
        Bounds Bounds { get; }
    }
}
