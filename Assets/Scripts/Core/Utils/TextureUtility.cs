using UnityEngine;

namespace Mechanizer
{
    public static class TextureUtility
    {
        public static Texture2D ResizeTexture(Texture2D originalTexture, int newWidth, int newHeight)
        {
            RenderTexture renderTexture = RenderTexture.GetTemporary(newWidth, newHeight);
            Graphics.Blit(originalTexture, renderTexture);


            Texture2D texture = new Texture2D(newWidth, newHeight, TextureFormat.ARGB32, false);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(Vector2.zero, new Vector2Int(newWidth, newHeight)), 0, 0);
            texture.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTexture);
            return texture;
        }
    }
}
