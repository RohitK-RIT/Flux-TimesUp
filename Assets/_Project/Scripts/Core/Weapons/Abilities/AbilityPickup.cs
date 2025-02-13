using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.UI;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public class AbilityPickup : MonoBehaviour, IPickupItem
    {
        [SerializeField] private AbilityType abilityType;
        public void OnItemPickup()
        {
            var playerWeaponController = LevelSceneController.Instance.Player.WeaponController;
            // Check if the player has an ability.
            if (!playerWeaponController.CurrentAbility)
            {
                // If not, switch to the new ability.
                playerWeaponController.SwitchAbility(abilityType);
            }
            // Check if the player has the same ability.
            else if (playerWeaponController.CurrentAbility.Type != abilityType)
            {
                // If not, show the ability selection page.
                LevelSceneController.Instance.AbilitySelectionPage.NextAbilityType = abilityType;
                LevelSceneController.Instance.AbilitySelectionPage.Show();
            }

            // Destroy the pickup item.
            Destroy(gameObject);
        }
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var msg = "You switched to " + abilityType.ToString() + " ability.";
                LevelSceneController.Instance.playerHUD.ShowPickupFeedback(msg);
                LevelSceneController.Instance.playerHUD.ShowAbilityHUD(abilityType);
            }
        }
    }
}