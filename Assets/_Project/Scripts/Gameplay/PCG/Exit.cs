using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.PCG {
    
    /// <summary>
    /// Represents an exit point in a room, used for connecting corridors.
    /// </summary>
    public class Exit : MonoBehaviour {
        public Vector3 localPosition; // Position relative to the room's origin
        public Vector3 worldPosition; // Absolute position in the world
        public bool isConnected; // Whether this exit has been connected
    }
}