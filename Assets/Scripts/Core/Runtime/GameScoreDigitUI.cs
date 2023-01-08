using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanizer
{
    public class GameNumberUI : MonoBehaviour
    {
        private string _text;
        private List<Digit> _digits;
        [SerializeField] private RectTransform _digitParent;
        [SerializeField] private Digit _digitPrefab;
        [SerializeField] private float _popAnimationSpeed = 3f;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                UpdateUI();
            }
        }

        private void CreateUI(char[] digits)
        {
            _digits = new List<Digit>();
            for (int i = 0; i < digits.Length; i++)
            {
                var digitInstance = Instantiate(_digitPrefab, _digitParent, false);
                _digits.Add(digitInstance);
            }
        }

        private void ResizeUI(char[] digits)
        {
            if (digits.Length > _digits.Count)
            {
                int diff = digits.Length - _digits.Count;
                for (int i = 0; i < diff; i++)
                {
                    Digit digitInstance = Instantiate(_digitPrefab, _digitParent, false);
                    _digits.Add(digitInstance);
                }
            }
            else if (digits.Length < _digits.Count)
            {
                int diff = _digits.Count - digits.Length;
                for (int i = 0; i < diff; i++)
                {
                    int index = _digits.Count - 1;
                    Digit digit = _digits[index];
                    Destroy(digit.gameObject);
                    _digits.RemoveAt(index);
                }
            }
        }

        private void UpdateUI()
        {
            char[] digits = Text.ToCharArray();
            if (_digits == null)
            {
                CreateUI(digits);
            }
            ResizeUI(digits);
            for (int i = 0; i < digits.Length; i++)
            {
                int digit = digits[i] - '0';
                _digits[i].SetDigit(digit);
            }
        }

        public void PopAnimation()
        {
            if (_digits == null)
                return;

            StartCoroutine(Routine());
            IEnumerator Routine()
            {
                float time = 0.0f;
                Color c1 = Color.white;
                Color c2 = Color.yellow;
                Color c3 = Color.white;

                Vector3 v1 = Vector3.one;
                Vector3 v2 = new Vector3(1.2f, 1.2f);
                Vector3 v3 = Vector3.one;
                while (time < 1.0f)
                {
                    time += Time.deltaTime * _popAnimationSpeed;
                    Color cl1 = Color.Lerp(c1, c2, time);
                    Color cl2 = Color.Lerp(c2, c3, time);
                    Color cl3 = Color.Lerp(cl1, cl2, time);

                    Vector3 vl1 = Vector3.Lerp(v1, v2, time);
                    Vector3 vl2 = Vector3.Lerp(v2, v3, time);
                    Vector3 vl3 = Vector3.Lerp(vl1, vl2, time);
                    foreach (Digit digit in _digits)
                    {
                        digit.Color = cl3;
                        digit.transform.localScale = vl3;
                    }
                    yield return null;
                }
            }
        }
    }
}