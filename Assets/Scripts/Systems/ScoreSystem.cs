using Character;
using Components;
using Mirror;

namespace Systems
{
    public class ScoreSystem : NetworkBehaviour
    {
        private static ScoreSystem _instance;
        public static ScoreSystem Instance => _instance;

        private string _scoreText;
        private Player[] _players;

        private void Awake()
        {
            _instance = this;
        }

        public void CollectData(uint id)
        {
            _players ??= FindObjectsOfType<Player>();
            if (_players.Length < id) _players = FindObjectsOfType<Player>();
            
            _scoreText = "";
            foreach (var player in _players)
                _scoreText += $"{player.NameComponent.PlayerName} - {player.Score}\n";
            
            ApplyData();
        }

        private void ApplyData()
        {
            if (_players == null) return;
            
            foreach (var player in _players)
                player.GetComponent<ScoreComponent>().UpdateScore(_scoreText);
        }
    }
}