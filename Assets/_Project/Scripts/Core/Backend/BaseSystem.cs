using System;
using UnityEngine;

namespace _Project.Scripts.Core.Backend
{
    /// <summary>
    /// Base class for all systems in the game.
    /// </summary>
    public abstract class BaseSystem<T> : MonoBehaviour where T : BaseSystem<T>
    {
        /// <summary>
        /// Determines if the system should be persistent between scenes.
        /// </summary>
        protected abstract bool IsPersistent { get; }

        /// <summary>
        /// The instance of the system.
        /// </summary>
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            // If there is already an instance of the system, destroy the new one.
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // If not, set the instance to this system and make it persistent between scenes.
            Instance = this as T;
            if (IsPersistent)
                DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            // If instance is being destroyed set Instance property to null.
            if (Instance == this)
                Instance = null;
        }
    }
}