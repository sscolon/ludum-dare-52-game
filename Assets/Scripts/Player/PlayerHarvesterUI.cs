using System.Collections.Generic;
using UnityEngine;

namespace Mechanizer
{
    public class PlayerHarvesterUI : PlayerChildUI
    {
        private PlayerHarvester _playerHarvester;
        private List<PlayerHarvesterSlotUI> _slots;
        [SerializeField] private PlayerHarvesterSlotUI _harvesterSlotPrefab;
        [SerializeField] private RectTransform _harvesterSlotParent;
        private void Start()
        {
            _playerHarvester = Context.GetComponentInChildren<PlayerHarvester>();
            _playerHarvester.OnCollect += AddUI;
            _playerHarvester.OnClear += ClearUI;
            CreateUI();
        }

        private void OnDestroy()
        {
            _playerHarvester.OnCollect -= AddUI;
            _playerHarvester.OnClear -= ClearUI;
        }

        private void CreateUI()
        {
            if (_slots == null)
                _slots = new List<PlayerHarvesterSlotUI>();
            foreach (var slot in _slots)
                Destroy(slot.gameObject);
            _slots.Clear();
            for (int i = 0; i < _playerHarvester.ComponentCount; i++)
            {
                PlayerHarvesterSlotUI slot = Instantiate(_harvesterSlotPrefab, _harvesterSlotParent, false);
                slot.UpdateUI(null);
                _slots.Add(slot);
            }
        }

        private void ClearUI()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].UpdateUI(null);
            }
        }

        private void AddUI(Collectible collectible)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            for (int i = 0; i < _playerHarvester.ComponentSprites.Count; i++)
            {
                var sprite = _playerHarvester.ComponentSprites[i];
                _slots[i].UpdateUI(sprite);
            }
        }
    }
}