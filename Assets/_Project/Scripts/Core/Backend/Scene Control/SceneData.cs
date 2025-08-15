using System;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    [Serializable]
    public class SceneData
    {
#if UNITY_EDITOR
        [SerializeField] private SceneAsset scene;

        public void AddToAssetBundle(string bundleName)
        {
            if (!scene)
            {
                Debug.LogWarning("Scene is null, cannot assign to asset bundle.");
                return;
            }

            var path = AssetDatabase.GetAssetPath(scene);
            var importer = AssetImporter.GetAtPath(path);

            if (!importer)
            {
                Debug.LogError("Could not get AssetImporter for the scene at path: " + path, scene);
                return;
            }

            importer.assetBundleName = bundleName;
        }
#endif
    }
}