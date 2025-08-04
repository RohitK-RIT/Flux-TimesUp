using System.IO;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.GroupEnemyBehavior
{
    public class DataCollection : MonoBehaviour
    {
        public static DataCollection Instance;

        private string logPath;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                logPath = Path.Combine(Application.persistentDataPath, $"session_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv");
                File.WriteAllText(logPath, "Timestamp,Category,Event,Value,Extra\n"); // CSV header
                Debug.Log(Application.persistentDataPath);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Log(string category, string eventName, float value = 0f, string extra = "")
        {
            string time = Time.time.ToString("F2");
            string line = $"{time},{category},{eventName},{value},{extra}";
            File.AppendAllText(logPath, line + "\n");
        }
    }
}
