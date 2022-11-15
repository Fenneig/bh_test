using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public class ScoreComponent : NetworkBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Text _canvasScoreText;

        private void Awake()
        {
            gameObject.SetActive(true);
        }

        public void ShowScore()
        {
            _canvas.enabled = true;
        }
    
        public void CloseScore()
        {
            _canvas.enabled = false;
        }
        
        [Client]
        public void UpdateScore(string newScore)
        {
            _canvasScoreText.text = newScore;
        }
    }
}