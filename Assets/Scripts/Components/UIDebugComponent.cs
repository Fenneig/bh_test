using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public class UIDebugComponent : MonoBehaviour
    {
        [SerializeField] private Text _debugText;

        private static UIDebugComponent _instance;
        public static UIDebugComponent Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        public void ShowMessage(string message)
        {
            _debugText.text = message;
        }
    }
}