using UnityEngine;

namespace Mechanizer
{
    public class Throwable : Attack, IHarvested
    {
        private Vector2 _targetSpeed;
        private ThrowableState _state;
        [Header("Throwable Settings")]
        [SerializeField] private Rigidbody2D _throwableBody;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private bool _destroyAfterDamage = true;
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
                            Destroy(gameObject);
                        }
                    }
        
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
            transform.SetParent(null);
            _targetSpeed = direction * MovementSpeed;
            _state = ThrowableState.Thrown;
        }

        private enum ThrowableState
        {
            Idle = 0,
            Held = 1,
            Thrown = 2
        }
    }
}
