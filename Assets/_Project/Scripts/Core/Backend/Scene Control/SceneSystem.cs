using System;
using System.Collections;
using _Project.Scripts.Core.Backend.Asset_Bundle;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    /// <summary>
    /// System for loading scenes.
    /// </summary>
    public class SceneSystem : BaseSystem<SceneSystem>
    {
        protected override bool IsPersistent => true;

        public bool Initialized { get; private set; }

        /// <summary>
        /// Name of the loading scene.
        /// </summary>
        [SerializeField] private string loadingSceneName = "Loading Scene";

        [SerializeField] private SceneBundleData sceneBundleData;

        /// <summary>
        /// Coroutine for loading a scene.
        /// </summary>
        private Coroutine _loadSceneCoroutine;

        private IEnumerator Start()
        {
            yield return AssetBundleSystem.Instance.LoadBundleAsync(sceneBundleData.BundleName, success => Initialized = success);
        }

        /// <summary>
        /// Load a scene.
        /// </summary>
        /// <param name="request">struct describing scene detail to be loaded</param>
        public void LoadScene(SceneLoadRequest request)
        {
            // If there is a scene loading coroutine running, stop it.
            if (_loadSceneCoroutine != null)
                StopCoroutine(_loadSceneCoroutine);

            // Start a new coroutine to load the scene.
            _loadSceneCoroutine = StartCoroutine(LoadSceneCoroutine(request));
        }

        /// <summary>
        /// Coroutine for loading a scene.
        /// </summary>
        /// <param name="request">struct describing scene detail to be loaded</param>
        private IEnumerator LoadSceneCoroutine(SceneLoadRequest request)
        {
            // Load the loading scene first.
            var sceneOp = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
            yield return sceneOp;


            // Load the requested scene. If the scene is not found, log an error and return.
            sceneOp = SceneManager.LoadSceneAsync(request.SceneName, request.Mode);

            if (sceneOp == null)
            {
                Debug.LogError($"Failed to load scene {request.SceneName}");
                yield break;
            }

            // Else, wait until the requested scene is loaded.
            yield return sceneOp;
        }
    }
}