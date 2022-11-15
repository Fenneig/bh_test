using Mirror;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Components
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

        public string PlayerName
        {
            get => _playerName;
            set => _playerName = value;
        }

        public Color PlayerColor
        {
            get => _playerColor;
            set => _playerColor = value;
        }

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
            Color color = new Color(Random.Range(0f, 0.5f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            string newName = string.IsNullOrEmpty(PlayerName) ? gameObject.name : PlayerName;
            CmdSetupPlayer(newName, color);
        }

        [Command]
        private void CmdSetupPlayer(string playerName, Color color)
        {
            _playerName = playerName;
            _playerColor = color;
        }

        private void Update()
        {
            if (isLocalPlayer) return;
            _floatingName.transform.LookAt(Camera.main.transform);
        }
    }
}