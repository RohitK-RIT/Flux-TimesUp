using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public class AbilityPickup : MonoBehaviour, IInteractable
    {
        public AbilityType Type => abilityType;
        public string DisplayName => abilityType.ToString();

        [SerializeField] private AbilityType abilityType;


        public void OnPickup(PlayerController controller)
        {
            gameObject.SetActive(false);
        }

        public void OnDrop() { }

        public void OnHoverEnter(PlayerController controller)
        {
            if (controller.HandController.CurrentAbility) 
                return;
            
            //Get Ability
            if (controller.HandController.OnItemPicked(this))
                OnPickup(controller);
        }

        public void OnHoverExit() { }
    }
}