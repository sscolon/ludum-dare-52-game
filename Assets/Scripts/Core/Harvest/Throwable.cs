using System;
using UnityEngine;

namespace Mechanizer
{
    public class Throwable : Attack, IHarvested
    {
        private Vector2 _targetSpeed;
        private ThrowableState _state;
        private PlayerHarvester _harvester;
        private float _height;
        private float _gravity;

        [Header("Throwable Components")]
        [SerializeField] private Rigidbody2D _throwableBody;
        [SerializeField] private Transform _rotatorTransform;
        [SerializeField] private Transform _heightTransform;
        [SerializeField] private bool _destroyAfterDamage = true;

        [Header("Movement")]
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _rotationSpeed = 20f;

        [Header("Gravity")]
        [SerializeField] private float _throwHeight;
        [SerializeField] private float _throwHeightOffset;
        [SerializeField] private float _fallSpeed;

        public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }

        public event Action OnHarvest;
        public event Action OnThrow;
        public event Action OnDeath;
        private void OnValidate()
        {
            if (_throwableBody == null)
                _throwableBody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            switch (_state)
            {
                case ThrowableState.Held:
                    if (Input.GetMouseButtonDown(0))
                    {
                        ThrowToMouse();
                    }
                    break;
                case ThrowableState.Thrown:
                    _height -= Time.deltaTime * _gravity;
                    _gravity += _fallSpeed * Time.deltaTime;

                    _rotatorTransform.Rotate(new Vector3(0, 0, _rotationSpeed * Time.deltaTime));
                    _heightTransform.transform.localPosition = new Vector3(0, _height + _throwHeightOffset, 0);
                    if (_height <= 0f)
                    {
                        Kill();
                    }
                    break;
            }
        }

        private void FixedUpdate()
        {
            switch (_state)
            {
                case ThrowableState.Thrown:
                    _throwableBody.velocity = _targetSpeed; 
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (_state)
            {
                case ThrowableState.Thrown:
                    if (DoDamage(collision) || collision.gameObject.CompareTag("World"))
                    {
                        if (_destroyAfterDamage)
                        {
                            Kill();
                        }
                    }
        
                break;
            } 
        }

        public void OnHarvestInit(PlayerHarvester context)
        {
            _harvester = context;
            transform.SetParent(context.CraftTransform);
            transform.localScale = Vector3.one;
            _state = ThrowableState.Held;
            OnHarvest?.Invoke();
        }

        private void ThrowToMouse()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldMousePosition = Helpers.MainCamera.ScreenToWorldPoint(mousePosition);
            worldMousePosition.z = 0f;

            Vector3 direction = worldMousePosition - transform.position;
            direction = direction.normalized;
            ThrowInDirection(direction);
        }

        private void ThrowInDirection(Vector2 direction)
        {
            transform.SetParent(null);
            _targetSpeed = direction * MovementSpeed;
            _state = ThrowableState.Thrown;
            _height = _throwHeight;
            _gravity = 0f;
            _harvester.ClearComponents();
            OnThrow?.Invoke();
        }

        private void Kill()
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }

        private enum ThrowableState
        {
            Idle = 0,
            Held = 1,
            Thrown = 2
        }
    }
}
