using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class NetworkManagerLobby : NetworkRoomManager
    {
        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;

        private readonly List<NetworkRoomPlayerLobby> _roomPlayers = new List<NetworkRoomPlayerLobby>();
        public List<NetworkRoomPlayerLobby> RoomPlayers => _roomPlayers;

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

        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
        {
            foreach (var player in RoomPlayers)
                player.gameObject.SetActive(false);

            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
        }

        public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
        {
            GameObject player = Instantiate(playerPrefab);
            player.GetComponent<NameComponent>().PlayerName = roomPlayer.GetComponent<NetworkRoomPlayerLobby>().DisplayName;
            Transform startPosition = GetStartPosition();
            player.transform.position = startPosition.position;
            player.transform.rotation = startPosition.rotation;

            return player;
        }
    }
}