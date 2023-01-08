using UnityEngine;
using UnityEngine.UI;

namespace Mechanizer
{
    public class PlayerHarvesterSlotUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        public void UpdateUI(Sprite sprite)
        {
            _icon.sprite = sprite;
            if (sprite == null)
                _icon.color = Color.clear;
            else
                _icon.color = Color.white;
        }
    }
}