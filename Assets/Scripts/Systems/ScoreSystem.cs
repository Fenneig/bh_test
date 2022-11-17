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
        private bool _isPlayersListFull;

        private void Awake()
        {
            _instance = this;
        }

        [Command(requiresAuthority = false)]
        public void CmdCollectData() => CollectData();
        

        [ClientRpc]
        private void CollectData()
        {
            if (!_isPlayersListFull)
            {
                var playersNow = _players?.Length ?? 0;
                _players = FindObjectsOfType<Player>();
                _isPlayersListFull = playersNow == _players.Length;
            }
            
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