using System.Collections;
using UnityEngine;

namespace Mechanizer
{
    [RequireComponent(typeof(IDamageable))]
    public class DamageFeedback : MonoBehaviour
    {
        private static WaitForSeconds _flickerWait = new WaitForSeconds(0.1f);
        private Coroutine _flickerRoutine;
        private Material _originalMaterial;
        private IDamageable _damageable;
        [SerializeField] private Feedback _damageFeedback;
        [SerializeField] private Feedback _deathFeedback;

        [Header("Bounce Settings")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Material _spriteWhiteMaterial;
        [SerializeField] private Vector3 _flickerStartScale = new Vector3(0.8f, 1.2f, 1f);
        [SerializeField] private float _flickerSpeed = 15f;
        private void OnEnable()
        {
            if (_damageable == null)
                _damageable = GetComponent<IDamageable>();
            _originalMaterial = _spriteRenderer.material;
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
            DoFlicker();
        }

        private void DoFlicker()
        {
            if (_flickerRoutine != null)
                StopCoroutine(_flickerRoutine);
            _flickerRoutine = StartCoroutine(FlickerRoutine());
        }

        private IEnumerator FlickerRoutine()
        {
            _spriteRenderer.material = _spriteWhiteMaterial;
            _spriteRenderer.transform.localScale = _flickerStartScale;

            yield return _flickerWait;
            _spriteRenderer.material = _originalMaterial;
            yield return null;
            while (true)
            {
                _spriteRenderer.transform.localScale = Vector3.Lerp(_spriteRenderer.transform.localScale, Vector3.one, Time.deltaTime * _flickerSpeed);
                float distance = Vector3.Distance(_spriteRenderer.transform.localScale, Vector3.one);
                if (distance <= 0.05f)
                {
                    break;
                }
                yield return null;
            }

            _spriteRenderer.transform.localScale = Vector3.one;
            _flickerRoutine = null;
        }

        private void PlayDeathFeedback()
        {
            _deathFeedback.CreateFeedback(gameObject);
        }
    }
}
