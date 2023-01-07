using System;
using UnityEngine;

namespace Mechanizer
{
    [Serializable]
    public class Damage
    {
        [SerializeField] private float _value;
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}
