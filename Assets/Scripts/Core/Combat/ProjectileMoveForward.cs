using UnityEngine;

namespace Mechanizer
{
    public class ProjectileMoveForward : ProjectileBase
    {
        [SerializeField] private Rigidbody2D _projectileBody;
        [SerializeField] private float _movementSpeed;
        public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }
        private void OnValidate()
        {
            if (_projectileBody == null)
                _projectileBody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Vector2 direction = (Origin - Target).normalized;
            Vector2 velocity = direction * MovementSpeed;
            _projectileBody.velocity = velocity;
        }
    }
}