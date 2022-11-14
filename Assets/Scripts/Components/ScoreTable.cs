using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public class ScoreTable : NetworkBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Text _canvasScoreText;
    
        private readonly SyncDictionary<string, int> _scoreTable = new SyncDictionary<string, int>();

        private static ScoreTable _instance;
        public static ScoreTable Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            _scoreTable.Callback += OnScoreChange;
            gameObject.SetActive(true);
        }

        public override void OnStartClient()
        {
            _scoreTable.Callback += OnScoreChange;

            foreach (var score in _scoreTable)  
                OnScoreChange(SyncDictionary<string, int>.Operation.OP_ADD, score.Key, score.Value);
        }

        public void ShowScore()
        {
            _canvas.enabled = true;
        }
    
        public void CloseScore()
        {
            _canvas.enabled = false;
        }
    
        public void AddPlayerToTable(string playerName)
        {
            AddPlayerToTable(playerName, 0);
        }
    
        public void AddPlayerToTable(string playerName, int score)
        {
            OnScoreChange(SyncIDictionary<string, int>.Operation.OP_ADD, playerName, score);
        }
    
        public void RemovePlayerFromTable(string playerName)
        {
            if (_scoreTable.ContainsKey(playerName))
                OnScoreChange(SyncIDictionary<string, int>.Operation.OP_REMOVE, playerName, 0);
        }
    
        public void AddPoint(string playerName)
        {
            _scoreTable[playerName]++;
            UpdateCanvasText();
        }
    
        public void ResetScoreTable()
        {
            OnScoreChange(SyncIDictionary<string, int>.Operation.OP_REMOVE, "", 0);
            UpdateCanvasText();
        }
    
        public void RenamePlayer(string oldName, string newName)
        {
            int savedScore = _scoreTable[oldName];
            RemovePlayerFromTable(oldName);
            AddPlayerToTable(newName, savedScore);
        }
    
        private void UpdateCanvasText()
        {
            string result = "";
            foreach (var (playerName, points) in _scoreTable)
                result += $"{playerName} - {points}\n";

            _canvasScoreText.text = result;
        }
    
        private void OnDestroy()
        {
            _scoreTable.Callback -= OnScoreChange;
        }
    
        private void OnScoreChange(SyncDictionary<string, int>.Operation op, string playerName, int points)
        {
            switch (op)
            {
                case SyncIDictionary<string, int>.Operation.OP_ADD:
                    //_scoreTable.Add(playerName, points);
                    UpdateCanvasText();
                    break;
                case SyncIDictionary<string, int>.Operation.OP_SET:
                
                    UpdateCanvasText();
                    break;
                case SyncIDictionary<string, int>.Operation.OP_REMOVE:
                    //_scoreTable.Remove(playerName);
                    UpdateCanvasText();
                    break;
                case SyncIDictionary<string, int>.Operation.OP_CLEAR:
                    //foreach (var score in _scoreTable) _scoreTable[score.Key] = 0;
                    UpdateCanvasText();
                    break;
            }
        }
    }
}