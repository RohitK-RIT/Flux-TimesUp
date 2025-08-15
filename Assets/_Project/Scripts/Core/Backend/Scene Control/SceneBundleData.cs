using _Project.Scripts.Core.Backend.Asset_Bundle;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    [CreateAssetMenu(fileName = "Scene Bundle Data", menuName = "Bundle Data/Scene", order = 0)]
    public sealed class SceneBundleData : BundleData
    {
#if UNITY_EDITOR
        [SerializeField] private SceneAsset[] scenes;

        protected override void InternalConfigureBundle()
        {
            foreach (var scene in scenes)
            {
                AddToAssetBundle(BundleName, scene);
            }
        }
#endif
    }
}