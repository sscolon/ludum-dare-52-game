using UnityEngine;

namespace Mechanizer
{
    [SerializeField]
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
