using UnityEngine;

namespace Mechanizer
{
    public class Throwable : MonoBehaviour, IHarvested
    {
        private Vector2 _targetSpeed;
        private ThrowableState _state;
        [SerializeField] private Rigidbody2D _throwableBody;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;
        [SerializeField] private float _movementSpeed;
        public float Acceleration { get => _acceleration; set => _acceleration = value; }
        public float Deceleration { get => _deceleration; set => _deceleration = value; }
        public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }
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
            }
        }

        private void FixedUpdate()
        {
            switch (_state)
            {
                case ThrowableState.Thrown:
                    //Same code as player movement.
                    //Just make sure to really tune up the acceleration so it doesn't have a slow start.
                    //We're doing it this way incase we want to make certain changes later.
                    Vector2 diff = _targetSpeed - _throwableBody.velocity;
                    float accelX = (Mathf.Abs(_targetSpeed.x) > 0.01f) ? Acceleration : Deceleration;
                    float accelY = (Mathf.Abs(_targetSpeed.y) > 0.01f) ? Acceleration : Deceleration;

                    Vector2 movement = Vector2.zero;
                    movement.x = Mathf.Abs(diff.x) * accelX * Mathf.Sign(diff.x) * Acceleration;
                    movement.y = Mathf.Abs(diff.y) * accelY * Mathf.Sign(diff.y) * Acceleration;
                    _throwableBody.velocity = movement;
                    break;
            }
        }

        public void OnHarvestInit(PlayerHarvester context)
        {
            transform.SetParent(context.CraftTransform);
            transform.localScale = Vector3.one;
            _state = ThrowableState.Held;
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
            _targetSpeed = direction * MovementSpeed;
        }

        private enum ThrowableState
        {
            Idle = 0,
            Held = 1,
            Thrown = 2
        }
    }
}
