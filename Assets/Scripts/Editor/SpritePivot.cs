using Mechanizer;
using UnityEditor;
using UnityEngine;

public static class SpritePivot
{
    [MenuItem("Mechanizer/Center Local Sprite")]
    public static void CenterLocalSprite()
    {
        if (Selection.activeGameObject != null)
        {
            SpriteRenderer renderer = Selection.activeGameObject.GetComponent<SpriteRenderer>();

            Vector2 center = new Vector2(0.5f, 0.5f);
            Sprite targetSprite = renderer.sprite;
            if (targetSprite == null)
            {
                return;
            }

            Vector2 localPivot = SpriteUtility.GetLocalPivot(targetSprite);  
            Vector2 newPosition = -1 * (center - localPivot);
            renderer.transform.localPosition = newPosition;
        }
    }
}
