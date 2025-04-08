using _Project.Scripts.Core.Backend;
using _Project.Scripts.Core.Backend.Scene_Control;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class LobbyPage : UIPage
    {
        public override PageID ID => PageID.LobbyPage;

        [SerializeField] private string gameSceneName;
        [SerializeField] private TMP_Text lobbyCodeText;
        [SerializeField] private TMP_Text playerCountText;
        [SerializeField] private Button continueButton;

        private void OnEnable()
        {
            // // Set the lobby code text to the current session name
            // lobbyCodeText.text = NetworkSystem.Instance.CurrentSessionInfo.Name;
            //
            // // Set the continue button interactable based on whether the player has scene authority
            // continueButton.interactable = NetworkSystem.Instance.IsSceneAuthority;
            // continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnDisable()
        {
            // Remove the listener from the continue button
            continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }

        private void OnContinueButtonClicked()
        {
            // Load the game scene
            // SceneSystem.Instance.LoadNetworkScene(new SceneLoadRequest(gameSceneName, LoadSceneMode.Single));
        }

        private void Update()
        {
            // Update the player count text
            // playerCountText.text = NetworkSystem.Instance.CurrentSessionInfo.PlayerCount.ToString();
        }
    }
}