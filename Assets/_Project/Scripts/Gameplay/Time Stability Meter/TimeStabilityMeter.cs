using UnityEngine;

namespace _Project.Scripts.Gameplay.Time_Stability_Meter
{
    public class TimeStabilityMeter : MonoBehaviour
    {
        public float TimeStability { get; private set; }
        public float InitialTimeStability => initialTimeStability;
        public static TimeStabilityMeter Instance { get; private set; }
        [SerializeField] private float initialTimeStability = 100f;
        [SerializeField] private float decreaseRate = 0.01f;

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            TimeStability = initialTimeStability;
        }

        private void Update()
        {
            TimeStability -= decreaseRate * Time.deltaTime;
            TimeStability = Mathf.Clamp(TimeStability, 0, initialTimeStability);
        }
    }
}