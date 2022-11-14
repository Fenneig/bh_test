using Character;
using Mirror;

namespace Menu
{
    public class NetworkGamePlayerLobby : NetworkBehaviour
    {
        [SyncVar] private string _displayName = "";
        
        public string DisplayName
        {
            get => _displayName;
            [Server] set => _displayName = value;
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

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);
            
            Room.GamePlayers.Add(this);
            
            GetComponent<Player>().OnStartLocalPlayer();
        }

        public override void OnStopClient()
        {
            Room.GamePlayers.Remove(this);
        }
    }
}