using System.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Asset_Bundle
{
    public abstract class PrefabBundleData : BundleData
    {
        public async Task<T> LoadPrefabAsync<T>(string assetPath) where T : MonoBehaviour
        {
            var prefab = await AssetBundleSystem.Instance.LoadAssetAsync<GameObject>(BundleName, assetPath);
            if (prefab && prefab.TryGetComponent<T>(out var componentPrefab))
                return componentPrefab;

            return null;
        }
    }
}