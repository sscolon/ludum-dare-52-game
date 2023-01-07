using System;
using UnityEngine;

namespace Mechanizer
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Health _health;
        public Health Health { get => _health; }

        public event Action OnDeath;
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
                OnDeath?.Invoke();
                Destroy(gameObject);
            }
        }

        public PartyTag GetParty()
        {
            return PartyTag.Enemy;
        }
    }
}
