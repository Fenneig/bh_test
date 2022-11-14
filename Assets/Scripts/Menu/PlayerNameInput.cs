using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class PlayerNameInput : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private Button _continueButton;

        public static string DisplayName { get; private set; }
        

        public void SetPlayerName(string playerName)
        {
            _continueButton.interactable = !string.IsNullOrEmpty(playerName);
        }

        public void SavePlayerName()
        {
            DisplayName = _nameInputField.text;
        }
    }
}