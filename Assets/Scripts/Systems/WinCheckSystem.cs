using System.Collections;
using Character;
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
        [SerializeField] private Transform[] _spawns;
        
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
                player.SwitchInputActivation();
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
            
            ResetScene();
        }

        private void ResetScene()
        {
            foreach (var player in _players)
            {
                player.transform.position = _spawns[Random.Range(0, _spawns.Length)].position;
                player.transform.rotation = _spawns[Random.Range(0, _spawns.Length)].rotation;

                player.Score = 0;
                
                player.SwitchInputActivation();
            }
        }
    }
}