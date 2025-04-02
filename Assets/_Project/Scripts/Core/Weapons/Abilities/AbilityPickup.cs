using System;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Player_Controllers;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public class AbilityPickup : MonoBehaviour, IPickable
    {
        public static event Action<PlayerController, AbilityType> OnAbilityPicked;
        [SerializeField] private AbilityType abilityType;

        [SerializeField] private TMP_Text abilityPickUpInstruction;

        private void Start()
        {
            abilityPickUpInstruction.text = "Press 'F' for \"" + abilityType + "\" Ability";
            abilityPickUpInstruction.gameObject.SetActive(false);
        }

        public void OnPickup(PlayerController playerController)
        {
            OnAbilityPicked?.Invoke(playerController, abilityType);
            gameObject.SetActive(false);
            Destroy(gameObject, 2f);
        }

        public void OnDrop() { }

        public void OnHoverEnter(PlayerController playerController)
        {
            if (!playerController.HandController.CurrentAbility)
            {
                //Get Ability
                OnPickup(playerController);
                return;
            }

            //Press F to pick up new ability
            abilityPickUpInstruction.gameObject.SetActive(true);
        }

        public void OnHoverExit()
        {
            abilityPickUpInstruction.gameObject.SetActive(false);
        }
    }
}