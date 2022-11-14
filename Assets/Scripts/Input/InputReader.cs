using Character;
using Components;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputReader : NetworkBehaviour
    {
        [SerializeField] private Player _player;

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
            _player.Dash();
        }

        [Client]
        public void OnShowScore(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer) return;

            if (context.started)  ScoreTable.Instance.ShowScore();
            if (context.canceled)  ScoreTable.Instance.CloseScore();
        }
    }
}