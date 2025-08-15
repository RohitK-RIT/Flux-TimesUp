#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Core.Backend.Asset_Bundle
{
    public class AssetBundleSystem : BaseSystem<AssetBundleSystem>
    {
#if UNITY_EDITOR
        [SerializeField] private BuildAssetBundleOptions options;
#endif
        protected override bool IsPersistent => true;

        private Dictionary<string, AssetBundle> _assetBundles;

        protected override void Awake()
        {
            base.Awake();
            _assetBundles = new Dictionary<string, AssetBundle>();
        }

        public IEnumerator LoadBundleAsync(string bundleName, Action<bool> onComplete = null)
        {
            var bundlePath = Path.Combine(Application.streamingAssetsPath, bundleName);
            if (!File.Exists(bundlePath))
            {
                Debug.LogError($"Asset Bundle {bundleName} does not exist at path: {bundlePath}");
                onComplete?.Invoke(false);
                yield break;
            }

            if (_assetBundles.ContainsKey(bundleName))
            {
                Debug.LogWarning($"Asset Bundle {bundleName} is already loaded.");
                onComplete?.Invoke(true);
                yield break;
            }

            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(bundlePath);
            yield return bundleLoadRequest;

            if (bundleLoadRequest.assetBundle)
            {
                _assetBundles.Add(bundleName, bundleLoadRequest.assetBundle);
                onComplete?.Invoke(true);
                yield break;
            }

            Debug.LogError($"Failed to load Asset Bundle: {bundleName}");
            onComplete?.Invoke(false);
        }

        public IEnumerator UnloadBundleAsync(string bundleName, Action<bool> onComplete = null, bool unloadAllLoadedObjects = true)
        {
            if (!_assetBundles.TryGetValue(bundleName, out var bundle))
            {
                Debug.LogError($"Asset Bundle {bundleName} is not loaded.");
                onComplete?.Invoke(false);
                yield break;
            }

            yield return bundle.UnloadAsync(unloadAllLoadedObjects);

            _assetBundles.Remove(bundleName);
            onComplete?.Invoke(true);
        }

        public IEnumerator LoadAssetAsync<T>(string bundleName, string assetPath, Action<T> onEnd) where T : Object
        {
            if (!_assetBundles.TryGetValue(bundleName, out var bundle))
            {
                onEnd?.Invoke(null);
                yield break;
            }

            if (!bundle.Contains(assetPath))
            {
                Debug.LogError($"{assetPath} not found in Asset Bundle {bundleName}.");
                onEnd?.Invoke(null);
                yield break;
            }

            var assetLoadRequest = bundle.LoadAssetAsync<T>(assetPath);
            yield return assetLoadRequest;

            if (assetLoadRequest.asset)
                onEnd?.Invoke(assetLoadRequest.asset as T);
        }

#if UNITY_EDITOR
        [ContextMenu("Remove All Asset Bundle Names")]
        private void RemoveAllAssetBundleNames()
        {
            var allBundleNames = AssetDatabase.GetAllAssetBundleNames();
            foreach (var bundleName in allBundleNames)
                AssetDatabase.RemoveAssetBundleName(bundleName, true);
        }

        [ContextMenu("Configure Asset Bundles")]
        private void ConfigureAssetBundles()
        {
            RemoveAllAssetBundleNames();

            var bundleDataAssets = AssetDatabase.FindAssets("t:BundleData", new[] { "Assets" });
            foreach (var assetGuid in bundleDataAssets)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
                var bundleData = AssetDatabase.LoadAssetAtPath<BundleData>(assetPath);

                if (bundleData)
                    bundleData.ConfigureBundle();
                else
                    Debug.LogWarning($"No BundleData found at path: {assetPath}");
            }
        }

        [ContextMenu("Build Asset Bundles")]
        private void BuildAssetBundles()
        {
            ConfigureAssetBundles();

            // Build the asset bundles and save them to the specified path.
            var outputPath = Application.streamingAssetsPath;
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            BuildPipeline.BuildAssetBundles(outputPath, options, EditorUserBuildSettings.activeBuildTarget);

            Debug.Log("Asset Bundles built and saved to: " + outputPath);
        }
#endif
    }
}