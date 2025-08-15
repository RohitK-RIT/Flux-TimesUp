#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

        public IEnumerator LoadAssetBundle(string bundleName)
        {
            var bundlePath = Path.Combine(Application.streamingAssetsPath, bundleName);
            if (!File.Exists(bundlePath))
            {
                Debug.LogError($"Asset Bundle {bundleName} does not exist at path: {bundlePath}");
                yield break;
            }

            if (_assetBundles.ContainsKey(bundleName))
            {
                Debug.LogWarning($"Asset Bundle {bundleName} is already loaded.");
                yield break;
            }

            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(bundlePath);
            yield return bundleLoadRequest;

            if (bundleLoadRequest.assetBundle)
                _assetBundles.Add(bundleName, bundleLoadRequest.assetBundle);
            else
                Debug.LogError($"Failed to load Asset Bundle: {bundleName}");
        }

        public IEnumerator UnloadAssetBundle(string bundleName)
        {
            if (!_assetBundles.TryGetValue(bundleName, out var bundle))
            {
                Debug.LogError($"Asset Bundle {bundleName} is not loaded.");
                yield break;
            }

            bundle.Unload(true);
            _assetBundles.Remove(bundleName);
        }

#if UNITY_EDITOR
        [ContextMenu("Configure Asset Bundles")]
        private void ConfigureAssetBundles()
        {
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