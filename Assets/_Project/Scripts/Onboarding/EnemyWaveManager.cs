using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Gameplay.PCG;
using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class EnemyWaveManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] waveOneEnemies;
        [SerializeField] private GameObject[] waveTwoEnemies;
        [SerializeField] private OnboardingFlowControl onboardingFlowControl;
        [SerializeField] private GameObject tsmToolTip;

        private EnemyDeathListener _enemyWaveOneDeathListener;
        private EnemyDeathListener _enemyWaveTwoDeathListener;
        private void Start()
        {
            var waveOneEnemyList = waveOneEnemies.Select(enemy => enemy.GetComponent<EnemyController>()).ToList();
            var waveTwoEnemyList = waveTwoEnemies.Select(enemy => enemy.GetComponent<EnemyController>()).ToList();
            _enemyWaveOneDeathListener = new EnemyDeathListener(waveOneEnemyList);
            _enemyWaveOneDeathListener.OnAllEnemiesDead += OnWaveOneEnemyDeath;
            _enemyWaveTwoDeathListener = new EnemyDeathListener(waveTwoEnemyList);
            _enemyWaveTwoDeathListener.OnAllEnemiesDead += OnWaveTwoEnemyDeath;
        }
        
        private void OnWaveOneEnemyDeath()
        {
            tsmToolTip.SetActive(true);
            StartCoroutine(PauseGameplay());
            _enemyWaveOneDeathListener.OnAllEnemiesDead -= OnWaveOneEnemyDeath;
        }
        
        private void OnWaveTwoEnemyDeath()
        {
            _enemyWaveTwoDeathListener.OnAllEnemiesDead -= OnWaveTwoEnemyDeath;
            onboardingFlowControl.MoveToNextOnboardingPhase();
            LevelSceneController.Instance.WinPage.SetActive(true);
        }
        private IEnumerator PauseGameplay()
        {
            Time.timeScale = 0; // Pause the game
            yield return new WaitForSecondsRealtime(5f);
            Time.timeScale = 1; // Resume the game
        }
    }
}
