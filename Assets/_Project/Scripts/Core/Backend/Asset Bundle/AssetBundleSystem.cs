#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

        public async Task<bool> LoadBundleAsync(string bundleName)
        {
            var bundlePath = Path.Combine(Application.streamingAssetsPath, bundleName);
            if (!File.Exists(bundlePath))
            {
                Debug.LogError($"Asset Bundle {bundleName} does not exist at path: {bundlePath}");
                return false;
            }

            if (_assetBundles.ContainsKey(bundleName))
            {
                Debug.LogWarning($"Asset Bundle {bundleName} is already loaded.");
                return true;
            }

            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(bundlePath);

            while (!bundleLoadRequest.isDone)
            {
                await Task.Yield(); // Yield until the bundle is loaded
            }

            if (bundleLoadRequest.assetBundle)
            {
                _assetBundles.Add(bundleName, bundleLoadRequest.assetBundle);
                return true;
            }

            Debug.LogError($"Failed to load Asset Bundle: {bundleName}");
            return false;
        }

        public async Task<bool> UnloadBundleAsync(string bundleName, bool unloadAllLoadedObjects = true)
        {
            if (!_assetBundles.TryGetValue(bundleName, out var bundle))
            {
                Debug.LogError($"Asset Bundle {bundleName} is not loaded.");
                return false;
            }

            var unloadOperation = bundle.UnloadAsync(unloadAllLoadedObjects);
            while (!unloadOperation.isDone)
                await Task.Yield(); // Yield until the bundle is unloaded

            _assetBundles.Remove(bundleName);
            return true;
        }

        public async Task<T> LoadAssetAsync<T>(string bundleName, string assetPath) where T : Object
        {
            if (!_assetBundles.TryGetValue(bundleName, out var bundle))
                return null;

            if (!bundle.Contains(assetPath))
            {
                Debug.LogError($"{assetPath} not found in Asset Bundle {bundleName}.");
                return null;
            }

            var assetLoadRequest = bundle.LoadAssetAsync<T>(assetPath);
            while (!assetLoadRequest.isDone)
                await Task.Yield(); // Yield until the asset is loaded

            if (assetLoadRequest.asset)
                return assetLoadRequest.asset as T;

            Debug.LogError($"{bundleName} bundle contains {assetPath}, but didn't load.");
            return null;
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