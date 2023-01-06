using UnityEngine;

namespace Mechanizer
{
    public class SpriteOutline : MonoBehaviour
    {
        private SpriteRenderer _target;
        [SerializeField] private SpriteRenderer[] _renderers;
        private void UpdateSprite()
        {
            if (_target == null)
            {
                return;
            }

            for (int i = 0; i < _renderers.Length; i++)
            {
                var renderer = _renderers[i];
                renderer.sprite = _target.sprite;
            }
        }

        private void UpdateOutline()
        {
            if (_target == null)
            {
                return;
            }

            transform.position = _target.transform.position;
        }

        public void Outline(SpriteRenderer target)
        {
            _target = target;
        }

        private void LateUpdate()
        {
            UpdateOutline();
            UpdateSprite();
        }
    }
}
