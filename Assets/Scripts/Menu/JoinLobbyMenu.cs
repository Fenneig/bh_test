using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class JoinLobbyMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby _networkManager;

        [Header("UI")] 
        [SerializeField] private GameObject _landingPagePanel;
        [SerializeField] private TMP_InputField _ipAddressInputField;
        [SerializeField] private Button _joinButton;

        private void OnEnable()
        {
            NetworkManagerLobby.OnClientConnected += HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
        }

        private void OnDisable()
        {
            NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
        }

        public void JoinLobby()
        {
            string ipAddress = _ipAddressInputField.text;
            
            _networkManager.networkAddress = ipAddress;
            _networkManager.StartClient();

            _joinButton.interactable = false;
        }

        private void HandleClientConnected()
        {
            _joinButton.interactable = true;

            gameObject.SetActive(false);
            _landingPagePanel.SetActive(false);
        }

        private void HandleClientDisconnected()
        {
            _joinButton.interactable = true;
        }
    }
}