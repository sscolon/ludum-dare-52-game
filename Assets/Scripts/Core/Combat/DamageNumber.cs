using System.Collections.Generic;
using UnityEngine;

namespace Mechanizer
{
    public class DamageNumber : MonoBehaviour
    {
        private float _time;
        private float _shrinkDelay;
        private float _shrinkDelayTime;
        private float _shrinkTime;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector3 _startScale;
        private Vector3 _endScale;
        private List<Digit> _digits;
        [SerializeField] private Digit _digitIndicatorPrefab;
        [SerializeField] private Transform _digitParent;
        [SerializeField] private float _spacing = 4 / 16f;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _maxDeviation = 1f;
        [SerializeField] private Vector3 _startPositionOffset;
        [SerializeField] private Vector3 _upOffset;
        public float ShrinkDelay
        {
            get
            {
                return _shrinkDelay;
            }
            set
            {
                _shrinkDelay = value;
            }
        }

        private void CreateUI(int length)
        {
            if (_digits == null)
                _digits = new List<Digit>();
            _digitParent.transform.localPosition = Vector3.zero;
            while (_digits.Count < length)
            {
                int index = _digits.Count;
                Digit indicator = GameObject.Instantiate(_digitIndicatorPrefab, _digitParent.transform.position, _digitIndicatorPrefab.transform.rotation);
                indicator.transform.SetParent(_digitParent);
                indicator.transform.localPosition = new Vector3(index * _spacing, 0);
                _digits.Add(indicator);
            }

            //Center UI
            float x = ((float)length / 2) * _spacing;
            _digitParent.transform.localPosition = new Vector3(x, 0f, 0f);
        }

        public void SetDamage(string damage)
        {
            char[] characters = damage.ToCharArray();
            CreateUI(characters.Length);
            for (int i = 0; i < characters.Length; i++)
            {
                char c = characters[i];
                if (char.IsDigit(c))
                {
                    int digit = c - '0';
                    _digits[i].SetDigit(digit);
                }
            }

            for (int i = 0; i < _digits.Count; i++)
            {
                if (i >= characters.Length)
                {
                    _digits[i].Clear();
                }
            }
        }

        private void OnEnable()
        {
            _digitParent.localScale = Vector3.one;
            float x = Random.Range(-_maxDeviation, _maxDeviation);
            float y = Random.Range(-_maxDeviation, _maxDeviation);
            Vector3 offset = new Vector2(x, y);
            _time = 0.0f;
            _startScale = Vector3.one;

            transform.position = transform.position + offset + _startPositionOffset;
            _startPosition = transform.position;
            _endPosition = transform.position + _upOffset;
            _shrinkTime = 0f;
            _shrinkDelayTime = 0f;
            _shrinkDelay = ShrinkDelay;
        }

        private void Update()
        {
            _time += Time.deltaTime * _speed;
            Vector3 pos = VectorUtility.SmoothStep(_startPosition, _endPosition, _time);
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, _time * 2);
            transform.position = pos;
            if (_time >= 1f)
            {
                _shrinkDelayTime += Time.deltaTime;
                if (_shrinkDelayTime >= _shrinkDelay)
                {
                    _shrinkTime += Time.deltaTime * _speed;
                    Vector3 s = VectorUtility.SmoothStep(_startScale, _endScale, _shrinkTime);
                    _digitParent.localScale = s;
                    if (_shrinkTime >= 1f)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
