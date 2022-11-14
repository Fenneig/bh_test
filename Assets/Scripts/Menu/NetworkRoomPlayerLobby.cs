using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class NetworkRoomPlayerLobby : NetworkRoomPlayer
    {
        [Header("UI")]
        [SerializeField] private GameObject _lobbyUI;
        [SerializeField] private TMP_Text[] _playerNameTexts = new TMP_Text[4];
        [SerializeField] private TMP_Text[] _playerReadyTexts = new TMP_Text[4];
        [SerializeField] private Button _startGameButton;

        [SyncVar(hook = nameof(HandleDisplayNameChanged))] private string _displayName = "Loading...";
        [SyncVar(hook = nameof(HandleReadyStatusChanged))] private bool _isReady;
        
        private bool _isServer;

        public string DisplayName => _displayName;
        public bool IsReady => _isReady;
        public bool IsServer
        {
            set
            {
                _isServer = value;
                _startGameButton.gameObject.SetActive(value);
            }
        }

        private NetworkManagerLobby _room;

        public NetworkManagerLobby Room
        {
            get
            {
                if (_room != null) return _room;
                return _room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        public override void OnStartAuthority()
        {
            CommandSetDisplayName(PlayerNameInput.DisplayName);

            _lobbyUI.SetActive(true);
        }

        public override void OnStartClient()
        {
            Room.RoomPlayers.Add(this);

            UpdateDisplay();
        }

        public override void OnStopClient()
        {
            Room.RoomPlayers.Remove(this);

            UpdateDisplay();
        }

        public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();
        public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

        private void UpdateDisplay()
        {
            if (!isLocalPlayer)
            {
                foreach (var player in Room.RoomPlayers)
                {
                    gameObject.SetActive(false);
                    if (player.isLocalPlayer)
                    {
                        gameObject.SetActive(true);
                        player.UpdateDisplay();
                    }
                }
            }

            for (int i = 0; i < _playerNameTexts.Length; i++)
            {
                _playerNameTexts[i].text = "Waiting for player...";
                _playerReadyTexts[i].text = string.Empty;
            }

            for (int i = 0; i < Room.RoomPlayers.Count; i++)
            {
                _playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
                _playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady
                    ? "<color=green>Ready</color>"
                    : "<color=red>Not Ready</color>";
            }
        }

        public void HandleReadyToStart(bool readyToStart)
        {
            if (!_isServer) return;

            _startGameButton.interactable = readyToStart;
        }

        [Command]
        private void CommandSetDisplayName(string displayName)
        {
            _displayName = displayName;
        }

        [Command]
        public void CommandReadyUp()
        {
            _isReady = !_isReady;
            
            Room.NotifyPlayersOfReadyState();
        }

        [Command]
        public void CommandStartGame()
        {
            if (Room.RoomPlayers[0].connectionToClient != connectionToClient) return;
            
            Room.StartGame();
        }
    }
}