using UnityEngine;

namespace Mechanizer
{
    public class PlayerEntity : MonoBehaviour, IDamageable
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
            Destroy(gameObject);
        }

        public PartyTag GetParty()
        {
            return PartyTag.Player;
        }
    }
}
