using UnityEngine;

namespace Mechanizer
{
    public class AfterImageSprite : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        private bool _fadeOut = false;
        private float _fadeTime;
        private float _fadeStartTime;
        private float _alphaStart = 1f;
        private void Update()
        {
            if (!_fadeOut)
            {
                return;
            }

            float diff = Time.time - _fadeStartTime;
            float completionPercent = diff / _fadeTime;
            float newAlpha = _alphaStart - (completionPercent * _alphaStart);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);

            if (newAlpha <= 0)
            {
                _fadeOut = false;
                Destroy(gameObject);
            }
        }

        public void StartFade(float alphaStart, float fadeTime)
        {
            _fadeStartTime = Time.time;
            _fadeTime = fadeTime;
            _alphaStart = alphaStart;
            _fadeOut = true;
        }
    }
}
