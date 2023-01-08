using UnityEngine;
using UnityEngine.UI;

namespace Mechanizer
{
    public class DigitUI : MonoBehaviour
    {
        [SerializeField] private Image _spriteRenderer;
        [SerializeField] private Sprite[] _digits;
        public Color Color
        {
            set => _spriteRenderer.color = value;
        }

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