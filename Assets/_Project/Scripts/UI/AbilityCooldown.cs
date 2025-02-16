using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class AbilityCooldown : MonoBehaviour
    {
        [SerializeField] private TMP_Text cooldownText;
        [SerializeField] private Image cooldownImage;
        private bool isCooldownActive = false;

        private float cooldownTimer = 0f;
        private float cooldownDuration;

        private void Start()
        {
            cooldownText.gameObject.SetActive(false);
            cooldownImage.fillAmount = 0f;
        }
        private void Update()
        {
            if (isCooldownActive)
            {
                ApplyCooldown();
            }
        }

        private void ApplyCooldown()
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0f)
            {
                cooldownText.gameObject.SetActive(false);
                cooldownImage.fillAmount = 0f;
                isCooldownActive = false;
            }
            else
            {
                cooldownText.text = Mathf.CeilToInt(cooldownTimer).ToString();
                cooldownImage.fillAmount = cooldownTimer / cooldownDuration;
            }
        }
        public void ActivateCooldown(float cooldown)
        {
            cooldownText.gameObject.SetActive(true);
            cooldownText.text = Mathf.CeilToInt(cooldownDuration).ToString();
            cooldownTimer = cooldown;
            cooldownDuration = cooldown;
            isCooldownActive = true;
        }
    }
}