using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Backend.Scene_Control;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public class AbilityPickup : MonoBehaviour, IPickupItem
    {
        [SerializeField] private AbilityType abilityType;
        
        [SerializeField] private TMP_Text abilityPickUpInstruction;
        
        private void Start()
        {
            abilityPickUpInstruction.gameObject.SetActive(false);
        }

        public void OnItemPickup()
        {
            var playerWeaponController = LevelSceneController.Instance.Player.WeaponController;
            
            // Get the new ability. 
            if (!playerWeaponController.CurrentAbility)
            {
                playerWeaponController.SwitchAbility(abilityType);
                var msg = "You picked up " + abilityType.ToString();
                LevelSceneController.Instance.playerHUD.ShowPickupFeedback(msg);
                LevelSceneController.Instance.playerHUD.ShowAbilityHUD(abilityType);
            }

            if(playerWeaponController.CurrentAbility.Type != abilityType)
            {
                LevelSceneController.Instance.Player.WeaponController.SwitchAbility(abilityType);
                var msg = "You switched to " + abilityType.ToString();
                LevelSceneController.Instance.playerHUD.ShowPickupFeedback(msg);
                LevelSceneController.Instance.playerHUD.ShowAbilityHUD(abilityType);
            }
            // Destroy the pickup item.
            Destroy(gameObject);
        }
        
        public void OnItemEnterRange()
        {
            if (CheckForCurrentAbility())
            {
                //Press F to pick up new ability
                abilityPickUpInstruction.gameObject.SetActive(true);
                abilityPickUpInstruction.text = "Press 'F' for \"" + abilityType.ToString() + "\" Ability";
            }
            else
            {
                //Get Ability
                this.OnItemPickup();
            }
        }

        public void OnItemExitRange()
        {
            abilityPickUpInstruction.gameObject.SetActive(false);
        }
        
        private bool CheckForCurrentAbility()
        {
            return LevelSceneController.Instance.Player.WeaponController.CurrentAbility;
        }
    }
}