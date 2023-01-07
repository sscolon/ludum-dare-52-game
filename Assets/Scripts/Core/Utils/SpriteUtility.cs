using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mechanizer
{
    public static class SpriteUtility
    {
        public static Vector2 GetLocalCenter(SpriteRenderer renderer)
        {
            Vector2 center = new Vector2(0.5f, 0.5f);
            Sprite targetSprite = renderer.sprite;
            if (targetSprite == null)
            {
                return Vector2.zero;
            }

            Vector2 localPivot = GetLocalPivot(targetSprite);
            Vector2 newPosition = -1 * (center - localPivot);
            return newPosition;
        }

        public static Vector3 GetLocalPivot(Sprite sprite)
        {
            if (sprite == null)
            {
                return new Vector3(0.5f, 0.5f);
            }

            Bounds bounds = sprite.bounds;
            var pivotX = -bounds.center.x / bounds.extents.x / 2 + 0.5f;
            var pivotY = -bounds.center.y / bounds.extents.y / 2 + 0.5f;
            return new Vector3(pivotX, pivotY);
        }
    }
}
