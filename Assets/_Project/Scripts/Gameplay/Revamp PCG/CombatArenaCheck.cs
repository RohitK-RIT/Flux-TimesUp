using _Project.Scripts.Gameplay.Time_Stability_Meter;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Revamp_PCG
{
    public class CombatArenaCheck : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player entered combat arena");
                TimeStabilityMeter.Instance.PauseTimeStabilityMeter = false;
            }
        }
    }
}
