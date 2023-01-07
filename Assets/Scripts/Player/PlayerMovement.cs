using UnityEngine;

namespace Mechanizer
{
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerMovementState _state;
        private Vector2 _input;
        private Vector2 _lastDodgePosition;
        private Vector2 _dodgeRollInput;
        private Vector2 _targetSpeed;
        private float _traveledDodgeDistance;
        private StateAnimator _animator;
        [Header("Components")]
        [SerializeField] private Rigidbody2D _playerBody;
        [SerializeField] private Transform _flipTransform;
        [SerializeField] private Transform _rotatorTransform;

        [Header("Stats")]
        [SerializeField] private float _acceleration = 12f;
        [SerializeField] private float _deceleration = 4f;
        [SerializeField] private float _movementSpeed = 8f;
        [SerializeField] private float _dodgeRollSpeed;
        [SerializeField] private float _dodgeRollDistance;
        [SerializeField] private float _flipSpeed;
        public float Acceleration { get => _acceleration; set => _acceleration = value; }
        public float Deceleration { get => _deceleration; set => _deceleration = value; }
        public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }
        public float DodgeRollSpeed { get => _dodgeRollSpeed; set => _dodgeRollSpeed = value; }
        public float DodgeRollDistance { get => _dodgeRollDistance; set => _dodgeRollDistance = value; }
        private void OnValidate()
        {
            if (_playerBody == null)
                _playerBody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _animator = new StateAnimator(GetComponent<Animator>());
        }

        private void EnterIdle()
        {
            _state = PlayerMovementState.Idle;
        }

        private void EnterRun()
        {
            _state = PlayerMovementState.Run;
        }

        private void EnterDodgeRoll()
        {
            _lastDodgePosition = transform.position;
            _traveledDodgeDistance = 0f;
            _dodgeRollInput = _input;
            _state = PlayerMovementState.Dodge_Roll;
        }

        private void UpdateFlipTransform()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldMousePosition = Helpers.MainCamera.ScreenToWorldPoint(mousePosition);
            worldMousePosition.z = 0.0f;

            if (worldMousePosition.x < transform.position.x)
            {
                Vector3 targetScale = new Vector3(-1f, 1f, 1f);
                _flipTransform.localScale = Vector3.Lerp(_flipTransform.localScale, targetScale, _flipSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 targetScale = new Vector3(1f, 1f, 1f);
                _flipTransform.localScale = Vector3.Lerp(_flipTransform.localScale, targetScale, _flipSpeed * Time.deltaTime);
            }
        }

        private void Update()
        {
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = Input.GetAxisRaw("Vertical");
            _input = _input.normalized;


            switch (_state)
            {
                case PlayerMovementState.Idle:
                    UpdateFlipTransform();
                    _animator.Play("idle");
                    _targetSpeed = Vector2.zero;
                    if (_input.x != 0f || _input.y != 0f)
                    {
                        EnterRun();
                    }

                    break;
                case PlayerMovementState.Run:
                    UpdateFlipTransform();
                    _animator.Play("run");
                    _targetSpeed = _input * MovementSpeed;
                    if (_input.x == 0f && _input.y == 0f)
                    {
                        EnterIdle();
                    }
                    //Dash to Dodge Roll
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        EnterDodgeRoll();
                    }

                    break;
                case PlayerMovementState.Dodge_Roll:

                    _animator.Play("dodge");
                    _targetSpeed = _dodgeRollInput * DodgeRollSpeed;

                    float distance = Vector3.Distance(transform.position, _lastDodgePosition);
                    _traveledDodgeDistance += distance;

                    Vector3 mousePosition = Input.mousePosition;
                    Vector3 worldMousePosition = Helpers.MainCamera.ScreenToWorldPoint(mousePosition);
                    worldMousePosition.z = 0.0f;
                    float z = (_traveledDodgeDistance / _dodgeRollDistance) * -360f;
                    if (worldMousePosition.x < transform.position.x)
                        z *= -1f;

                    _rotatorTransform.localRotation = Quaternion.Euler(0, 0, z);
                    if(_traveledDodgeDistance >= DodgeRollDistance)
                    {
                        _rotatorTransform.localRotation = Quaternion.identity;
                          EnterRun();
                    }
                    _lastDodgePosition = transform.position;
                    break;
            }
        }

        private void FixedUpdate()
        {
            //Simple fluid movement code!
            Vector2 diff = _targetSpeed - _playerBody.velocity;
            float accelX = (Mathf.Abs(_targetSpeed.x) > 0.01f) ? Acceleration : Deceleration;
            float accelY = (Mathf.Abs(_targetSpeed.y) > 0.01f) ? Acceleration : Deceleration;

            Vector2 movement = Vector2.zero;
            movement.x = Mathf.Abs(diff.x) * accelX * Mathf.Sign(diff.x) * Acceleration;
            movement.y = Mathf.Abs(diff.y) * accelY * Mathf.Sign(diff.y) * Acceleration;
            _playerBody.AddForce(movement);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (_state)
            {
                //We need to put this here so that players don't get softlocked by dashing into a wall.
                case PlayerMovementState.Dodge_Roll:
                    EnterRun();
                    break;
            }
        }

        private enum PlayerMovementState
        {
            Idle = 0,
            Run = 1,
            Dodge_Roll = 2,
        }
    }
}