using UnityEngine;

namespace Mechanizer
{
    public class PlayerEntity : MonoBehaviour, IDamageable
    {
        [SerializeField] private Health _health;
        public Health Health { get => _health; }
        private void OnEnable()
        {
            Health.OnValueChange += Die;
        }

        private void OnDisable()
        {
            Health.OnValueChange -= Die;
        }

        private void Die(float healthValue)
        {
            if (healthValue <= 0f)
            {
                //Do death thing here.
                Destroy(gameObject);
            }
        }
    }
}
