using _Project.Scripts.Movables;
using UnityEngine;

namespace _Project.Scripts.Unity
{
    public class PathAdapter : MonoBehaviour, IPath
    {
        [SerializeField] private BezierSolution.BezierSpline _spline;
        public Vector3 GetPositionFromTime(in float time)
        {
            return _spline.GetPoint(time);
        }

        public float GetTime(Vector3 position)
        {
            _spline.FindNearestPointToLine(position, position, out _, out var time);
            return time;
        }
    }
}
