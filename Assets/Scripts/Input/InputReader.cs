using Character;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputReader : NetworkBehaviour
    {
        [SerializeField] private Player _player;

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (!isOwned) return;
            _player.MoveDirection = context.ReadValue<Vector2>();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (!isOwned) return;
            _player.Dash();
        }
    }
}