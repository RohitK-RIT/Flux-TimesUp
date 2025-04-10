using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Player_Controllers;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public class AbilityPickup : MonoBehaviour, IInteractable
    {
        public AbilityType Type => abilityType;

        [SerializeField] private AbilityType abilityType;
        [SerializeField] private TMP_Text abilityPickUpInstruction;

        private void Start()
        {
            abilityPickUpInstruction.text = $"Press 'F' for \"{abilityType}\" Ability";
            abilityPickUpInstruction.gameObject.SetActive(false);
        }

        public void OnPickup(PlayerController controller)
        {
            gameObject.SetActive(false);
        }

        public void OnDrop() { }

        public void OnHoverEnter(PlayerController controller)
        {
            if (!controller.HandController.CurrentAbility)
            {
                //Get Ability
                if (controller.HandController.OnItemPicked(this))
                    OnPickup(controller);
            }
            else
            {
                //Press F to pick up new ability
                abilityPickUpInstruction.gameObject.SetActive(true);
            }
        }

        public void OnHoverExit()
        {
            abilityPickUpInstruction.gameObject.SetActive(false);
        }
    }
}