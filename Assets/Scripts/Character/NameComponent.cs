using Mirror;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Character
{
    public class NameComponent : NetworkBehaviour
    {
        [SerializeField] private TextMeshPro _playerNameText;
        [SerializeField] private GameObject _floatingName;

        [SerializeField] private Renderer _playerMaterialClone;

        [SyncVar(hook = nameof(OnNameChanged))] [SerializeField]
        private string _playerName;
        [SyncVar(hook = nameof(OnColorChanged))] [SerializeField]
        private Color _playerColor = Color.white;

        public string PlayerName => _playerName;

        private void OnNameChanged(string oldName, string newName)
        {
            _playerNameText.text = _playerName;
        }

        private void OnColorChanged(Color oldColor, Color newColor)
        {
            _playerNameText.color = newColor;
            _playerMaterialClone.material.color = newColor;
        }

        public override void OnStartLocalPlayer()
        {
            string playerName = "Player" + Random.Range(100, 999);
            Color color = new Color(Random.Range(0f, 0.5f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            CommandSetupPlayer(playerName, color);
        }

        [Command]
        public void CommandSetupPlayer(string playerName, Color color)
        {
            _playerName = playerName;
            _playerColor = color;
        }

        private void Update()
        {
            if (isLocalPlayer)
                return;

            _floatingName.transform.LookAt(Camera.main.transform);
        }
    }
}