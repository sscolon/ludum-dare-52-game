using UnityEngine;

namespace Mechanizer
{
    [RequireComponent(typeof(ProjectileBase))]
    public class ProjectileFeedback : MonoBehaviour
    {
        private ProjectileBase _projectile;
        [SerializeField] private Feedback _spawnFeedback;
        [SerializeField] private Feedback _deathFeedback;
        private void OnEnable()
        {
            if (_projectile == null)
                _projectile = GetComponent<ProjectileBase>();
            _spawnFeedback.CreateFeedback(gameObject);
            _projectile.OnDeath += PlayDeathFeedback;
        }

        private void OnDisable()
        {
            _projectile.OnDeath -= PlayDeathFeedback;
        }

        private void PlayDeathFeedback()
        {
            _deathFeedback.CreateFeedback(gameObject);
        }
    }
}