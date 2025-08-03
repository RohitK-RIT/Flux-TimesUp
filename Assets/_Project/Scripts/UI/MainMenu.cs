using _Project.Scripts.Core.Backend.Scene_Control;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI
{
    public class MainMenu : UIPage
    {
        public override PageID ID => PageID.MainMenu;

        [SerializeField] private string gameSceneName;
        [SerializeField] private string tutorialSceneName = "Onboarding Scene";

        public void OnStartButtonClicked()
        {
            SceneSystem.Instance.LoadScene(new SceneLoadRequest(gameSceneName, LoadSceneMode.Single));
        }

        public void OnTutorialButtonClicked()
        {
            SceneSystem.Instance.LoadScene(new SceneLoadRequest(tutorialSceneName, LoadSceneMode.Single));
        }

        public void OnMultiPlayButtonClicked()
        {
            UIManager.Instance.SwitchPage(PageID.SessionsPage);
        }

        /// <summary>
        /// Called when the quit button is clicked
        /// </summary>
        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}