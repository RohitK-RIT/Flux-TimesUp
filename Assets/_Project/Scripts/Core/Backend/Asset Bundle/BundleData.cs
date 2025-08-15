#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Asset_Bundle
{
    public abstract class BundleData : ScriptableObject
    {
        public string BundleName => !string.IsNullOrEmpty(bundleName) ? bundleName : name;

        [SerializeField] private string bundleName;
#if UNITY_EDITOR
        [SerializeField] private bool includeInBuild = true;

        public void ConfigureBundle()
        {
            if (includeInBuild)
                InternalConfigureBundle();
        }

        protected abstract void InternalConfigureBundle();

        protected static void AddToAssetBundle(string bundleName, Object obj)
        {
            if (!obj)
            {
                Debug.LogWarning("Object is null, cannot assign to asset bundle.", obj);
                return;
            }

            var path = AssetDatabase.GetAssetPath(obj);
            var importer = AssetImporter.GetAtPath(path);

            if (!importer)
            {
                Debug.LogError($"Could not get AssetImporter for the Object at path: {path}", obj);
                return;
            }

            importer.assetBundleName = bundleName;
        }
#endif
    }
}