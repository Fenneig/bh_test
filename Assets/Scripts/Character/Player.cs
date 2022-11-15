using Character.Movement;
using Cinemachine;
using Components;
using Mirror;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class Player : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private DashComponent _dashComponent;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private NameComponent _nameComponent;
        [SerializeField] private ScoreComponent _scoreComponent;
        [Header("Settings")]
        [SerializeField] private float _speed;
        [SerializeField] private float _turnSmoothTime = 0.1f;
        [SerializeField] private float _drag = 2f;

        private float _turnSmoothVelocity;
        private Vector2 _moveDirection;
        private bool _isGrounded;
        private RaycastHit _slopeHit;
        private Transform _camera;
        private CinemachineFreeLook _cinemachine;
        
        private const float SpeedMultiplier = 5f;
        private const float RayCastMultiplier = 0.6f;
        private const float MaxSlopeAngle = 45f;
        
        [SyncVar(hook = nameof(OnScoreChanged))] public int Score;

        public Vector2 MoveDirection
        {
            set => _moveDirection = value;
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }
        
        public PlayerInput Input => _input;

        public NameComponent NameComponent => _nameComponent;

        public override void OnStartClient()
        {
            if (!isLocalPlayer) return;
            _camera = Camera.main.transform;
            _cinemachine = FindObjectOfType<CinemachineFreeLook>();
            _cinemachine.LookAt = gameObject.transform;
            _cinemachine.Follow = gameObject.transform;
            _input.enabled = true;
            ScoreSystem.Instance.CollectData(netId);
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            SpeedControl();

            _isGrounded = Physics.Raycast(transform.position, Vector3.down, _collider.height * RayCastMultiplier,
                _groundMask);
            _rigidbody.drag = _isGrounded ? _drag : 0f;
        }

        private void FixedUpdate()
        {
            Vector3 direction = new Vector3(_moveDirection.x, 0f, _moveDirection.y).normalized;
            if (direction.magnitude > 0)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
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
            if (!Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _collider.height * RayCastMultiplier))
                return false;

            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < MaxSlopeAngle && angle != 0;
        }

        private Vector3 GetSlopeMovement(Vector3 moveDirection) => 
            Vector3.ProjectOnPlane(moveDirection, _slopeHit.normal).normalized;

        public void Dash()
        {
            if (_dashComponent != null) _dashComponent.Dash();
        }

        public void ShowScore() => _scoreComponent.ShowScore();

        public void CloseScore() => _scoreComponent.CloseScore();

        public void OnScoreChanged(int oldValue, int newValue)
        {
            ScoreSystem.Instance.CollectData(netId);
            WinCheckSystem.Instance.WinCheck(_nameComponent.PlayerName, newValue);
        }
        
    }
}