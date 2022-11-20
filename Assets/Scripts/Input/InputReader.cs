using Character;
using Character.Movement;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputReader : NetworkBehaviour
    {
        [SerializeField] private Player _player;
        private DashComponent _playerDashComponent;

        private void Start()
        {
            _playerDashComponent = _player.GetComponent<DashComponent>();
        }

        [Client]
        public void OnMovement(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer) return;
            _player.MoveDirection = context.ReadValue<Vector2>();
        }

        [Client]
        public void OnDash(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer) return;
            if (_playerDashComponent != null && context.started)
                _playerDashComponent.Dash();
        }
    }
}