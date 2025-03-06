using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    //General class to manage the game
    public class LevelSceneController : BaseSystem<LevelSceneController>
    {
        [SerializeField] public PlayerHUD playerHUD;
        protected override bool IsPersistent => false;
        public LocalPlayerController Player => player;
        public GameObject WinPage => winPage;
        public GameObject LoosePage => loosePage;

        [SerializeField] private GameObject pauseMenuPage, winPage, loosePage; // Drag your game scene UI panel here

        [Space(25f), Header("Players in Scene")] [SerializeField]
        private LocalPlayerController player; // Drag your player here

        private bool _isPaused; // Variable to check if the game is paused
        
        public EnemyController BossEnemy { get; set; }

        private void Update()
        {
            // Check if the player presses the pause key (Escape in this case)
            if (
#if UNITY_EDITOR
                Keyboard.current[Key.M].wasPressedThisFrame
#else
                Keyboard.current[Key.Escape].wasPressedThisFrame
#endif
            )
            {
                if (_isPaused)
                    Resume();
                else
                    Pause();
            }

            if (player.CurrentHealth <= 0) // Check if the player is dead
                GameOver(false);
            /*else if (Array.TrueForAll(enemies, enemy => enemy.CurrentHealth <= 0)) // Check if all enemies are dead.
                GameOver(true);*/
            else if (BossEnemy)
            {
                if(BossEnemy.CurrentHealth <= 0)
                    GameOver(true);
            }
        }

        private void OnDestroy()
        {
            // This is to prevent the game from freezing after returning from the menu scene.
            Time.timeScale = 1f; // Unfreeze the game;
        }

        private void GameOver(bool win)
        {
            Cursor.visible = true; // Show the cursor
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            if (win)
            {
                winPage.SetActive(true);
            }
            else
            {
                loosePage.SetActive(true);
            }
            PauseGame();
            //Time.timeScale = 0f; // Freeze the game
        }

        // Call this function to resume the game
        public void Resume()
        {
            pauseMenuPage.SetActive(false); // Hide pause menu
            ResumeGame();
        }

        // Call this function to pause the game
        private void Pause()
        {
            PauseGame();
            pauseMenuPage.SetActive(true); // Show pause menu
        }

        private void PauseGame()
        {
            Cursor.visible = true; // Show the cursor
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Time.timeScale = 0f; // Freeze the game
            _isPaused = true; // Update pause state
        }

        private void ResumeGame()
        {
            Cursor.visible = false; // Show the cursor
            Cursor.lockState = CursorLockMode.Locked; // Unlock the cursor
            Time.timeScale = 1f; // Unfreeze the game
            _isPaused = false; // Update pause state
        }

        // Function to restart the game and go back to the main menu
        public void BackToMainMenu()
        {
            // Unfreeze the game in case it's paused
            Time.timeScale = 1f;

            // Replace "MainMenuScene" with the name of your actual main menu scene
            SceneSystem.Instance.LoadScene(new SceneLoadRequest("UI", LoadSceneMode.Single));
        }

        //Function to exit the game when the quit button is clicked
        public void QuitGame()
        {
            // Check if we are running in the editor
#if UNITY_EDITOR
            // If in the editor, stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // If in a built version, quit the application
                Application.Quit();
#endif
        }
    }
}