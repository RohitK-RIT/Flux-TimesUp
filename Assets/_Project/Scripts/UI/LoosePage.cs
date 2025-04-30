using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.UI
{
    public class LoosePage : MonoBehaviour
    {
        [SerializeField] private TMP_Text loseText;

        int _randomRecordingNumber;
        private void Start()
        {
            _randomRecordingNumber = (int)Random.Range(0, 100);
        }

        private void Update()
        {
            if (gameObject.activeInHierarchy)
            {
                loseText.text = "Recording #" + _randomRecordingNumber + ": Failure";
            }
        }
    }
}
