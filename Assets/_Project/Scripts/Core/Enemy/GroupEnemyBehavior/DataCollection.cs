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
                string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                logPath = Path.Combine(desktopPath, $"game_analytics_log_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv");
                //logPath = Path.Combine(Application.persistentDataPath, $"session_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv");
                File.WriteAllText(logPath, "Category,Event,Value,Extra\n"); // CSV header
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Log(string category, string eventName, float value = 0f, string extra = "")
        {
            string line = $"{category},{eventName},{value},{extra}";
            File.AppendAllText(logPath, line + "\n");
        }
    }
}
