using Character;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private Player _player;

        public void OnMovement(InputAction.CallbackContext context) =>
            _player.MoveDirection = context.ReadValue<Vector2>();

        public void OnDash(InputAction.CallbackContext context) => _player.Dash();
    }
}