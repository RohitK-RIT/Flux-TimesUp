using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    /// <summary>
    /// Controller for the title scene.
    /// </summary>
    public class TitleSceneController : MonoBehaviour
    {
        /// <summary>
        /// Name of the main menu scene.
        /// </summary>
        [SerializeField] private string mainMenuSceneName;

        private IEnumerator Start()
        {
            // Wait for 2.5 seconds.
            yield return new WaitForSeconds(2.5f);

            // Wait till asset bundles are initialized.
            yield return new WaitUntil(() => SceneSystem.Instance.Initialized);

            // Load the main menu scene
            LoadMainMenuScene();
        }

        /// <summary>
        /// Load the main menu scene.
        /// </summary>
        private void LoadMainMenuScene()
        {
            SceneSystem.Instance.LoadScene(new SceneLoadRequest(mainMenuSceneName, LoadSceneMode.Single));
        }
    }
}