using UnityEngine;

namespace Mechanizer
{
    public class Digit : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _digits;
        public void SetDigit(int digit)
        {
            if (digit < 0 || digit > 9)
                return;

            _spriteRenderer.sprite = _digits[digit];
        }

        public void Clear()
        {
            _spriteRenderer.sprite = null;
        }
    }
}