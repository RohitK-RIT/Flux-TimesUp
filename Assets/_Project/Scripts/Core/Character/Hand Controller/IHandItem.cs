using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Character.Hand_Controller
{
    public interface IHandItem
    {
        // A smart way to get the gameObject and transform of the item
        public GameObject gameObject { get; }
        public Transform transform { get; }

        public PlayerController CurrentPlayerController { get; }

        /// <summary>
        /// Function called when the item is picked up.
        /// </summary>
        /// <param name="currentPlayerController">the player controller that will control the item</param>
        public void OnPickup(PlayerController currentPlayerController);

        /// <summary>
        /// Function called when the item is dropped.
        /// </summary>
        public void OnDrop();

        /// <summary>
        /// Start attacking.
        /// </summary>
        public void BeginUse();

        /// <summary>
        /// End attacking.
        /// </summary>
        public void EndUse();

        /// <summary>
        /// Function called when the item is equipped.
        /// </summary>
        public virtual void OnEquip() { }

        /// <summary>
        /// Function called when the item is unequipped.
        /// </summary>
        public virtual void OnUnequip() { }

        /// <summary>
        /// Function to set the layer of the item and all its children recursively.
        /// </summary>
        /// <param name="gameObj">the root gameObject for which layer has to be changed</param>
        /// <param name="layer">the layer to which the gameObject and it's children to be set</param>
        protected static void SetLayerRecursive(GameObject gameObj, LayerMask layer)
        {
            gameObj.layer = layer;
            foreach (Transform childTransform in gameObj.transform)
            {
                SetLayerRecursive(childTransform.gameObject, layer);
            }
        }
    }
}