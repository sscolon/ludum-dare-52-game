using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Mechanizer
{
    [Serializable]
    public class Hitbox
    {
        private ContactFilter2D _filter2D = new ContactFilter2D();
        public Vector2 center;
        public HitboxType hitboxType;
        public float circleRadius;
        public Vector2 squareSize;
        public float SizeModifier { get; set; } = 1f;
        public void InitFilter()
        {
            _filter2D.useTriggers = true;
        }

        public void Overlap(Transform ctx, List<Collider2D> results)
        {
            float angle = ctx.eulerAngles.z;
            Vector3 offset = center;
            Vector3 offsetPoint = ctx.transform.position + offset;
            Vector2 finalPoint = VectorUtility.RotatePointAroundPivot(offsetPoint, ctx.transform.position, ctx.eulerAngles);
            switch (hitboxType)
            {
                case HitboxType.Circle:
                    Physics2D.OverlapCircle(finalPoint, circleRadius * SizeModifier, _filter2D, results);
                    break;
                case HitboxType.Square:
                    Physics2D.OverlapBox(finalPoint, squareSize * SizeModifier, angle, _filter2D, results);
                    break;
            }
        }
#if UNITY_EDITOR
        public void DrawGizmos(Transform ctx, Color color)
        {
            Handles.color = Color.red;
            float angle = ctx.eulerAngles.z;
            Vector3 offset = center;
            Vector3 offsetPoint = ctx.transform.position + offset;
            Vector2 finalPoint = VectorUtility.RotatePointAroundPivot(offsetPoint, ctx.transform.position, ctx.eulerAngles);
            float thickness = 16f;
            switch (hitboxType)
            {
                case HitboxType.Circle:
                    Handles.DrawWireDisc(finalPoint, Vector3.forward, circleRadius * SizeModifier, thickness);
                    break;
                case HitboxType.Square:
                    var orientation = Quaternion.Euler(0, 0, angle);
                    Vector2 right = orientation * Vector2.right * squareSize.x * SizeModifier / 2f;
                    Vector2 up = orientation * Vector2.up * squareSize.y * SizeModifier / 2f;

                    var topLeft = finalPoint + up - right;
                    var topRight = finalPoint + up + right;
                    var bottomRight = finalPoint - up + right;
                    var bottomLeft = finalPoint - up - right;

                    Handles.DrawBezier(topLeft, topRight, topLeft, topRight, color, null, thickness);
                    Handles.DrawBezier(topRight, bottomRight, topRight, bottomRight, color, null, thickness);
                    Handles.DrawBezier(bottomRight, bottomLeft, bottomRight, bottomLeft, color, null, thickness);
                    Handles.DrawBezier(bottomLeft, topLeft, bottomLeft, topLeft, color, null, thickness);
                    break;
            }
        }
#endif
    }
}
