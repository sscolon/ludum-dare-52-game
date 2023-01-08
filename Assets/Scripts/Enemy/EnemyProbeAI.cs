using UnityEngine;

namespace Mechanizer
{
    public class EnemyProbeAI : EnemyBaseAI
    {
        private Vector2 _targetSpeed;
        private State _state;
        [Header("Probe AI")]
        [SerializeField] private float _attackRange;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;
        [SerializeField] private ProjectileBase _projectilePrefab;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _spriteRotator;
        [SerializeField] private Transform _projectileRotator;
        [SerializeField] private Transform _projectileSpawn;
        [SerializeField] private Transform _projectileTarget;
        protected override void UpdateAI()
        {
            _projectileRotator.rotation = Helpers.LookAt(_projectileRotator, Target.transform);
            _spriteRotator.rotation = Helpers.LookAt(Target.transform, _projectileRotator);
            _spriteRenderer.flipY = Target.transform.position.x < transform.position.x;
            switch (_state)
            {
                case State.Idle:
                    Animator.Play("idle");
                    _targetSpeed = Vector2.zero;
                    _state = State.Chase;
                    break;
                case State.Chase:
                    Animator.Play("run");
                    if (DistanceToTarget > _attackRange)
                    {
                        _targetSpeed = DirectionToTarget * _movementSpeed;
                    }

                    if (DistanceToTarget <= _attackRange)
                    {
                        _state = State.Attack;
                    }
                    break;
                case State.Attack:
                    Animator.Play("attack");
                    _targetSpeed = Vector2.zero;
                    break;
            }
        }

        protected override void FixedUpdateAI()
        {
            Move(_targetSpeed, _acceleration, _deceleration);
        }

        private void Attack()
        {
            ProjectileBase projectile = Instantiate(_projectilePrefab, _projectileSpawn.position, _projectilePrefab.transform.rotation);
            
            projectile.transform.rotation = Helpers.LookAt(Target.transform, _projectileRotator);
            projectile.Origin = _projectileSpawn.position;
            projectile.Target = _projectileTarget.position;
        }

        private void EndAttack()
        {
            _state = State.Chase;
        }

        private enum State
        {
            Idle = 0,
            Chase = 1,
            Attack = 2,
        }
    }
}