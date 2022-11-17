using System.Collections;
using Character;
using Menu;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public class WinCheckSystem : NetworkBehaviour
    {
        [SerializeField] private int _scoreToWin;
        [SerializeField] private GameObject _winCanvas;
        [SerializeField] private Text _winText;
        [SerializeField] private int _timeToRestart;
        
        private static WinCheckSystem _instance;
        public static WinCheckSystem Instance => _instance;

        private Player[] _players;
        
        private void Awake()
        {
            _instance = this;
        }

        public void WinCheck(string playerName, int score)
        {
            if (score != _scoreToWin) return;
            ShowWinMessage(playerName);
            _players ??= FindObjectsOfType<Player>();
            foreach (var player in _players)
                player.Input.enabled = false;
            StartCoroutine(RestartGame());
        }
        
        private void ShowWinMessage(string playerName)
        {
            _winCanvas.SetActive(true);
            _winText.text = $"{playerName} has won!";
        }

        private IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(_timeToRestart);
            _winCanvas.SetActive(false);

            Cursor.lockState = CursorLockMode.Confined;

            foreach (var player in _players)
                if (player != null) NetworkServer.Destroy(player.gameObject);

            var networkManager = FindObjectOfType<NetworkManagerLobby>();

            networkManager.RestartGame();
        }
    }
}