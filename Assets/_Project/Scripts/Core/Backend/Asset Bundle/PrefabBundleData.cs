using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Asset_Bundle
{
    public abstract class PrefabBundleData : BundleData
    {
        protected IEnumerator LoadPrefabAsync<T>(string assetPath, Action<T> onComplete) where T : MonoBehaviour
        {
            GameObject prefab = null;

            yield return AssetBundleSystem.Instance.LoadAssetAsync<GameObject>(BundleName, assetPath, go => prefab = go);

            if (prefab && prefab.TryGetComponent<T>(out var componentPrefab))
            {
                onComplete?.Invoke(componentPrefab);
            }
            else
            {
                Debug.LogError($"Failed to load prefab of type {typeof(T)} from asset path: {assetPath}");
                onComplete?.Invoke(null);
            }
        }
    }
}