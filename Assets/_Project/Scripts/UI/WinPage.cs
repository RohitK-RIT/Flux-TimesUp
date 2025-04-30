using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class WinPage : MonoBehaviour
    {
   
        [SerializeField] private TMP_Text winText;

        int _randomRecordingNumber;
        private void Start()
        {
            _randomRecordingNumber = (int)Random.Range(0, 100);
        }

        private void Update()
        {
            if (gameObject.activeInHierarchy)
            {
                winText.text = "Recording #" + _randomRecordingNumber + ": Success";
            }
        }
    }
}
