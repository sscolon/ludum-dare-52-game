using UnityEngine;

namespace Mechanizer
{
    public static class VectorUtility
    {
        public static Vector3 SmoothStep(Vector3 start, Vector3 end, float t)
        {
            float x = Mathf.SmoothStep(start.x, end.x, t);
            float y = Mathf.SmoothStep(start.y, end.y, t);
            float z = Mathf.SmoothStep(start.z, end.z, t);
            return new Vector3(x, y, z);
        }
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            var dir = point - pivot;
            dir = Quaternion.Euler(angles) * dir;
            point = dir + pivot;
            return point;
        }
    }
}