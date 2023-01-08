using System.Collections.Generic;
using UnityEngine;

namespace Mechanizer
{
    public class PlayerHealthUI : PlayerChildUI
    {
        private List<PlayerHealthPipUI> _pips;
        [SerializeField] private PlayerHealthPipUI _pipUIPrefab;
        [SerializeField] private RectTransform _pipParent;

        private void Start()
        {
            int pips = (int)Context.Health.MaxValue;
            CreateUI(pips);
            Context.Health.OnValueChange += UpdateUI;
        }

        private void OnDestroy()
        {
            Context.Health.OnValueChange -= UpdateUI;
        }

        private void CreateUI(int pips)
        {
            if (_pips == null)
            {
                _pips = new List<PlayerHealthPipUI>();
            }

            foreach (var pip in _pips)
                Destroy(pip.gameObject);

            _pips.Clear();
            for (int i = 0; i < pips; i++)
            {
                PlayerHealthPipUI pip = Instantiate(_pipUIPrefab, _pipParent, false);
                _pips.Add(pip);
            }
        }

        private void UpdateUI(float healthValue)
        {
            int pipCount = (int)Context.Health.MaxValue;
            if (pipCount != _pips.Count)
            {
                CreateUI(pipCount);
            }

            int filledPips = (int)healthValue;
            for (int i = 0; i < _pips.Count; i++)
            {
                var pip = _pips[i];
                if (i < filledPips)
                {
                    pip.Fill();
                }
                else
                {
                    pip.Clear();
                }
            }
        }
    }
}
