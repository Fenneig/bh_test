using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class NetworkManagerLobby : NetworkRoomManager
    {
        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;

        private readonly List<NetworkRoomPlayerLobby> _roomPlayers = new List<NetworkRoomPlayerLobby>();
        private readonly List<NetworkGamePlayerLobby> _gamePlayers = new List<NetworkGamePlayerLobby>();
        public List<NetworkRoomPlayerLobby> RoomPlayers => _roomPlayers;
        public List<NetworkGamePlayerLobby> GamePlayers => _gamePlayers;

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            if (!RoomScene.Contains(SceneManager.GetActiveScene().name)) return;

            bool isServer = RoomPlayers.Count == 0;

            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab) as NetworkRoomPlayerLobby;

            roomPlayerInstance.IsServer = isServer;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            OnClientConnected?.Invoke();
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            OnClientDisconnected?.Invoke();
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

                RoomPlayers.Remove(player);

                NotifyPlayersOfReadyState();
            }

            base.OnServerDisconnect(conn);
        }

        public override void OnStopServer()
        {
            RoomPlayers.Clear();
        }

        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in RoomPlayers)
                player.HandleReadyToStart(IsReadyToStart());
        }

        private bool IsReadyToStart() => numPlayers >= minPlayers && RoomPlayers.All(player => player.IsReady);

        public void StartGame()
        {
            if (!RoomScene.Contains(SceneManager.GetActiveScene().name)) return;
            
            ServerChangeScene("InGameScene");
        }

        public override void ServerChangeScene(string newSceneName)
        {
            if (RoomScene.Contains(SceneManager.GetActiveScene().name))
            {
                for (int i = _roomPlayers.Count-1; i >= 0; i--)
                {
                    var conn = _roomPlayers[i].connectionToClient;
                    var gamePlayerInstance = Instantiate(playerPrefab);
                    
                    gamePlayerInstance.GetComponent<NetworkGamePlayerLobby>().DisplayName = _roomPlayers[i].DisplayName;

                    NetworkServer.Destroy(conn.identity.gameObject);

                    NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance);
                }
            }

            base.ServerChangeScene(newSceneName);
        }
        
        
    }
}