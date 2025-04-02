using UnityEngine;

namespace _Project.Scripts.Core.Backend.Helper
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Function to set the layer of the item and all its children recursively.
        /// </summary>
        /// <param name="obj">the root gameObject for which layer has to be changed</param>
        /// <param name="newLayer">the layer to which the gameObject and it's children to be set</param>
        public static void SetLayerRecursively(this GameObject obj, int newLayer)
        {
            if (!obj)
                return;

            // Set the layer of the object.
            obj.layer = newLayer;

            // Set the layer of all the children recursively.
            foreach (Transform child in obj.transform)
            {
                if (!child)
                    continue;

                SetLayerRecursively(child.gameObject, newLayer);
            }
        }

        public static void SetLayerRecursively(this GameObject obj, string newLayerName)
        {
            var layer = LayerMask.NameToLayer(newLayerName);
            
            SetLayerRecursively(obj, layer);
        }
    }
}