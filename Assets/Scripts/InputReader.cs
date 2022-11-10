using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private Player _player;

        public void OnMovement(InputAction.CallbackContext context) =>
            _player.MoveDirection = context.ReadValue<Vector2>();

        public void OnAttack(InputAction.CallbackContext context) => _player.Attack();
    }
}