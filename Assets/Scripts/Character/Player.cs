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
        [SerializeField] private PlayerInput _input;
        [SerializeField] private NameComponent _nameComponent;
        [SerializeField] private ScoreComponent _scoreComponent;

        private float _turnSmoothVelocity;
        private bool _isGrounded;
        private RaycastHit _slopeHit;
        private Transform _camera;
        private CinemachineFreeLook _cinemachine;

        public Vector2 MoveDirection { get; set; }

        [field: SyncVar(hook = nameof(OnScoreChanged))]
        public int Score { get; set; }

        public PlayerInput Input => _input;

        public NameComponent NameComponent => _nameComponent;

        public Transform CameraTransform => _camera;

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

        public void ShowScore() => _scoreComponent.ShowScore();

        public void CloseScore() => _scoreComponent.CloseScore();

        public void OnScoreChanged(int oldValue, int newValue)
        {
            ScoreSystem.Instance.CollectData(netId);
            WinCheckSystem.Instance.WinCheck(_nameComponent.PlayerName, newValue);
        }
        
    }
}