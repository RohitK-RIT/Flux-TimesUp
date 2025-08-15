using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace _Project.Scripts.Core.Backend
{
    public class AssetBundleSystem : BaseSystem<AssetBundleSystem>
    {
#if UNITY_EDITOR
        [SerializeField] private BuildAssetBundleOptions options;
#endif
        protected override bool IsPersistent => true;

        public bool Initialized { get; private set; }

        [SerializeField] private string[] assetBundleNames;

        private IEnumerator Start()
        {
            yield return LoadAssetBundles();
            Initialized = true;
        }

        private IEnumerator LoadAssetBundles()
        {
            // Load asset bundles from the StreamingAssets folder if they are not already loaded.
            if (assetBundleNames == null || assetBundleNames.Length == 0)
            {
                Debug.LogError("AssetBundle names are not populated. Please populate them in the editor or use the PopulateAssetBundleNames method.");
                yield break;
            }

            foreach (var bundleName in assetBundleNames)
            {
                var bundlePath = Path.Combine(Application.streamingAssetsPath, bundleName);
                if (!File.Exists(bundlePath))
                {
                    Debug.LogError($"Asset Bundle {bundleName} does not exist at path: {bundlePath}");
                    continue;
                }

                var bundleLoadRequest = AssetBundle.LoadFromFileAsync(bundlePath);
                yield return bundleLoadRequest;

                if (!bundleLoadRequest.assetBundle)
                {
                    Debug.LogError($"Failed to load Asset Bundle: {bundleName}");
                }
                else
                {
                    Debug.Log($"Successfully loaded Asset Bundle: {bundleName}");
                }
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Populate Asset Bundles Names")]
        private void PopulateAssetBundleNames()
        {
            var bundleNames = AssetDatabase.GetAllAssetBundleNames();
            assetBundleNames = new string[bundleNames.Length];
            bundleNames.CopyTo(assetBundleNames, 0);
            EditorUtility.SetDirty(this);

            Debug.Log("Populated Asset Bundle Names: " + string.Join(", ", assetBundleNames));
        }

        [ContextMenu("Build Asset Bundles")]
        private void BuildAssetBundles()
        {
            PopulateAssetBundleNames();
            if (assetBundleNames.Length == 0)
            {
                Debug.LogWarning("No asset bundles found to build. Please ensure you have assigned asset bundles to your assets.");
                return;
            }

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