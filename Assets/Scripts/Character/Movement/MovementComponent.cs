using Mirror;
using UnityEngine;

namespace Character.Movement
{
    public class MovementComponent : NetworkBehaviour
    {
        [Header("Movement settings")]
        [SerializeField] private float _speed;
        [SerializeField] private float _drag = 2f;
        [SerializeField] private float _turnSmoothTime = 0.1f;
        
        [Header("References")]
        [SerializeField] private Player _player;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private LayerMask _groundMask;

        private bool _isGrounded;
        private float _turnSmoothVelocity;
        private RaycastHit _slopeHit;
        
        private const float SpeedMultiplier = 5f;
        private const float GroundRayCastMultiplier = 0.6f;
        private const float MaxSlopeAngle = 45f;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }
        
        private void Update()
        {
            SpeedControl();

            _isGrounded = Physics.Raycast(transform.position, Vector3.down, _collider.height * GroundRayCastMultiplier,
                _groundMask);
            _rigidbody.drag = _isGrounded ? _drag : 0f;
        }

        private void FixedUpdate()
        {
            Vector3 direction = new Vector3(_player.MoveDirection.x, 0f, _player.MoveDirection.y).normalized;
            if (direction.magnitude > 0)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _player.CameraTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                    _turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

                moveDirection = OnSlope() ? GetSlopeMovement(moveDirection) : moveDirection;

                _rigidbody.AddForce(moveDirection * _speed * SpeedMultiplier, ForceMode.Force);
            }
        }

        private void SpeedControl()
        {
            if (OnSlope())
            {
                if (_rigidbody.velocity.magnitude > _speed)
                {
                    _rigidbody.velocity = _rigidbody.velocity.normalized * _speed;
                }
            }
            else
            {
                Vector3 flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
                if (flatVelocity.magnitude > _speed)
                {
                    Vector3 limitedVelocity = flatVelocity.normalized * _speed;
                    _rigidbody.velocity = new Vector3(limitedVelocity.x, _rigidbody.velocity.y, limitedVelocity.z);
                }
            }
        }

        private bool OnSlope()
        {
            if (!Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _collider.height * GroundRayCastMultiplier))
                return false;

            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < MaxSlopeAngle && angle != 0;
        }

        private Vector3 GetSlopeMovement(Vector3 moveDirection) => 
            Vector3.ProjectOnPlane(moveDirection, _slopeHit.normal).normalized;
    }
}