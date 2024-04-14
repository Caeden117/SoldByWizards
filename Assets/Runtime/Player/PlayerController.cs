using SoldByWizards.Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoldByWizards.Player
{
    public class PlayerController : MonoBehaviour, WizardInput.IPlayerActions
    {
        public Camera Camera => _camera;
        public bool IsGrounded => _grounded;

        private CapsuleCollider _capsuleCollider = null!;
        private Rigidbody _rigidbody = null!;
        private Camera _camera = null!;

        [field: SerializeField]
        public float Sensitivity { get; set; } = 1f;

        [SerializeField]
        private InputController _inputController = null!;

        [SerializeField]
        private LayerMask _collisionMask;

        [SerializeField]
        private float _defaultCameraHeight = 0.75f;

        [SerializeField]
        private float _crouchedCameraHeight = 0.4f;

        [SerializeField]
        private float _maxSpeed = 0f;

        [SerializeField]
        private float _groundAccel = 0f;

        [SerializeField]
        private float _groundDecel = 0f;

        [SerializeField]
        private float _airAccel = 0f;

        [SerializeField]
        private float _jumpForce = 0f;

        private bool _grounded = false;
        private bool _inputJumping = false;
        private bool _inputCrouching = false;
        private Vector2 _inputMovement = Vector2.zero;
        private List<Collision> _collisions = new();

        private void Start()
        {
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _rigidbody = GetComponent<Rigidbody>();
            _camera = GetComponentInChildren<Camera>();

            _inputController.Input.Player.AddCallbacks(this);

            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Stop() => _rigidbody.velocity = _rigidbody.angularVelocity = Vector3.zero;

        private void OnPlayerDeath()
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.drag = 1f;
        }

        private void Update()
        {
            if (!_inputController.Enabled || !_inputController.PlayerInputEnabled) return;

            var lookValue = _inputController.Input.Player.Look.ReadValue<Vector2>();
            lookValue *= Sensitivity * 0.1f;
            var angles = _camera.transform.localEulerAngles;
            angles.x -= lookValue.y;
            angles.y += lookValue.x;

            if (angles.x > 180)
                angles.x -= 360;

            angles.x = Mathf.Clamp(angles.x, -90f, 90f);

            _camera.transform.SetLocalPositionAndRotation(
                _camera.transform.localPosition.WithY(_inputCrouching ? _crouchedCameraHeight : _defaultCameraHeight),
                Quaternion.Euler(angles));
        }

        private void FixedUpdate()
        {
            CheckGrounded();

            var velocity = _rigidbody.velocity;
            var yVelocity = velocity.y;
            velocity.y = 0.0f;

            if (_inputJumping && _grounded)
            {
                _grounded = false;
                yVelocity = _jumpForce;
            }

            var yaw = _camera.transform.localEulerAngles.y * Mathf.Deg2Rad;
            var cosYaw = Mathf.Cos(yaw);
            var sinYaw = Mathf.Sin(yaw);
            var inputDir = new Vector3(_inputMovement.x, 0f, _inputMovement.y).normalized;
            var direction = new Vector3(cosYaw * inputDir.x + sinYaw * inputDir.z, 0f, -sinYaw * inputDir.x + cosYaw * inputDir.z);

            // Attempting to move
            if (direction != Vector3.zero)
            {
                var dot = Vector3.Dot(velocity, direction);

                // Counter-strafing
                if (dot < 0.0f)
                {
                    velocity -= direction * dot;
                    dot = 0.0f;
                }

                // Grounded accelerate
                if (_grounded)
                {
                    velocity += _groundAccel * Time.fixedDeltaTime * direction;
                    var speed = velocity.magnitude;
                    if (speed > _maxSpeed)
                        velocity = velocity / speed * _maxSpeed;
                }
                // Air accelerate
                else if (dot < _airAccel * Time.fixedDeltaTime)
                    velocity += _airAccel * Time.fixedDeltaTime * direction;
            }
            // Giving no input & grounded
            else if (velocity != Vector3.zero && _grounded)
            {
                // Decelerate
                var speed = velocity.magnitude;
                direction = velocity / speed;
                speed = Mathf.Max(0.0f, speed - _groundDecel * Time.fixedDeltaTime);
                velocity = direction * speed;
            }

            velocity.y = yVelocity;

            _rigidbody.velocity = velocity;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        private void CheckGrounded()
        {
            var radius = _capsuleCollider.radius;
            var ray = new Ray(transform.position + new Vector3(0f, _capsuleCollider.height * -0.5f + radius, 0f), Vector3.down);
            _grounded = Physics.SphereCast(ray, radius - 0.05f, 0.051f, _collisionMask);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
                _inputJumping = true;
            else if (context.canceled)
                _inputJumping = false;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            _inputMovement = context.performed ? context.ReadValue<Vector2>() : Vector2.zero;
        }

        /*
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.performed)
                _inputCrouching = true;
            else if (context.canceled)
                _inputCrouching = false;
        }*/
    }
}
