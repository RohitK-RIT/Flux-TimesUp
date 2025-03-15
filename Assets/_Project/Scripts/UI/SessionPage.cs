using _Project.Scripts.Core.Backend;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class SessionPage : UIPage
    {
        public override PageID ID => PageID.SessionsPage;

        [SerializeField] private TMP_Text lobbyCodeText;
        [SerializeField] private TMP_InputField lobbyCodeInputField;
        [SerializeField] private TMP_Text statusText;

        public async void OnCreateButtonClicked()
        {
            // Start the game in host mode
            // var status = await NetworkSystem.Instance.CreateSession();

            // UpdateStatus(lobbyCodeText, status.Item1, status.Item2);
        }

        public async void OnJoinButtonClicked()
        {
            var statusFlag = false;
            var message = "Invalid lobby code";

            if (!string.IsNullOrEmpty(lobbyCodeInputField.text) && lobbyCodeInputField.text.Length > 4)
            {
                // Start the game in client mode
                // var status = await NetworkSystem.Instance.JoinSession(lobbyCodeInputField.text);

                // statusFlag = status.Item1;
                // message = status.Item2;
            }

            // UpdateStatus(statusText, statusFlag, message);
        }

        private void UpdateStatus(TMP_Text statusTextField, bool status, string message)
        {
            statusTextField.text = message;
            if (status)
            {
                statusTextField.color = Color.green;
                UIManager.Instance.SwitchPage(PageID.LobbyPage);
            }
            else
            {
                statusTextField.color = Color.red;
            }
        }
    }
}