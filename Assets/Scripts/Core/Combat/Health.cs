using System;
using UnityEngine;

namespace Mechanizer
{
    [Serializable]
    public class Health
    {
        [SerializeField] private float _value;
        [SerializeField] private float _maxValue;
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _value = Mathf.Clamp(_value, 0, MaxValue);
                OnValueChange?.Invoke(_value);
            }
        }

        public float MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
                _value = Mathf.Clamp(_value, 0, MaxValue);
                OnMaxValueChange?.Invoke(_maxValue);
            }
        }

        public event Action<float> OnValueChange;
        public event Action<float> OnMaxValueChange;
        public event Action<float> OnDamaged;
        public event Action OnDeath;
        public void TakeDamage(Damage damage)
        {
            Value -= damage.Value;
            OnDamaged?.Invoke(damage.Value);
            if(Value <= 0)
            {
                OnDeath?.Invoke();
            }
        }
    }
}
