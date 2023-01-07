using UnityEngine;

namespace Mechanizer
{
    [RequireComponent(typeof(Throwable))]
    public class ThrowableFeedback : MonoBehaviour
    {
        private Throwable _throwable;
        [SerializeField] private Feedback _harvestFeedback;
        [SerializeField] private Feedback _throwFeedback;
        [SerializeField] private Feedback _deathFeedback;
        private void OnEnable()
        {
            if (_throwable == null)
                _throwable = GetComponent<Throwable>();
            _throwable.OnHarvest += PlayHarvestFeedback;
            _throwable.OnThrow += PlayThrowFeedback;
            _throwable.OnDeath += PlayDeathFeedback;
        }

        private void OnDisable()
        {
            _throwable.OnHarvest -= PlayHarvestFeedback;
            _throwable.OnThrow -= PlayThrowFeedback;
            _throwable.OnDeath -= PlayDeathFeedback;
        }

        private void PlayHarvestFeedback()
        {
            _harvestFeedback.CreateFeedback(gameObject);
        }

        private void PlayThrowFeedback()
        {
            _throwFeedback.CreateFeedback(gameObject);
        }

        private void PlayDeathFeedback()
        {
            _deathFeedback.CreateFeedback(gameObject);
        }
    }
}
