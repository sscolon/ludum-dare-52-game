using UnityEngine;
using UnityEngine.UI;

namespace Mechanizer
{
    public class PlayerHealthPipUI : MonoBehaviour
    {
        [SerializeField] private Sprite _filledSprite;
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private Image _icon;
        public void Fill()
        {
            _icon.sprite = _filledSprite;
        }

        public void Clear()
        {
            _icon.sprite = _emptySprite;
        }
    }
}
