using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mechanizer
{
    [RequireComponent(typeof(IDamageable))]
    public class DamageFeedback : MonoBehaviour
    {
        private IDamageable _damageable;
        [SerializeField] private Feedback _damageFeedback;
        [SerializeField] private Feedback _deathFeedback;
        private void OnEnable()
        {
            if (_damageable == null)
                _damageable = GetComponent<IDamageable>();
            _damageable.Health.OnDamaged += PlayDamageFeedback;
            _damageable.Health.OnDeath += PlayDeathFeedback;
        }

        private void OnDisable()
        {
            _damageable.Health.OnDamaged -= PlayDamageFeedback;
            _damageable.Health.OnDeath -= PlayDeathFeedback;
        }

        private void PlayDamageFeedback(float damageValue)
        {
            _damageFeedback.CreateFeedback(gameObject);
        }

        private void PlayDeathFeedback()
        {
            _deathFeedback.CreateFeedback(gameObject);
        }
    }
}
