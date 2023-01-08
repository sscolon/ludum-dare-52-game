using System;
using UnityEngine;

namespace Mechanizer
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Health _health;
        public Health Health { get => _health; }
        private void OnEnable()
        {
            Health.OnDeath += Die;
        }

        private void OnDisable()
        {
            Health.OnDeath -= Die;
        }

        private void Die()
        {
            GameManager.Main.Score += 150;
            Destroy(gameObject);
        }

        public PartyTag GetParty()
        {
            return PartyTag.Enemy;
        }
    }
}
