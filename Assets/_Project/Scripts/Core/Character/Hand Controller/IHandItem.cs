using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Character.Hand_Controller
{
    public interface IHandItem
    {
        // A smart way to get the gameObject and transform of the item
        public GameObject gameObject { get; }
        public Transform transform { get; }

        public PlayerController Owner { get; }

        /// <summary>
        /// Function called when use of item started.
        /// </summary>
        public void BeginUse();

        /// <summary>
        /// Function called when use of item ended.
        /// </summary>
        public void EndUse();

        /// <summary>
        /// Function called when the item is equipped.
        /// </summary>
        public void OnEquip();

        /// <summary>
        /// Function called when the item is unequipped.
        /// </summary>
        public void OnUnequip();
    }
}