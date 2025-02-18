using System;
using _Project.Scripts.Core.Backend.Ability;
using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.Core.Weapons.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class AbilitySelectionPage : UIPage
    {
        public override PageID ID => PageID.AbilitySelection;

        [NonSerialized] public AbilityType NextAbilityType;

        [SerializeField] private TMP_Text currentAbilityName;
        [SerializeField] private TMP_Text currentAbilityDescription;
        [SerializeField] private Image currentAbilityIcon;

        [Space(20), SerializeField] private TMP_Text nextAbilityName;
        [SerializeField] private TMP_Text nextAbilityDescription;
        [SerializeField] private Image nextAbilityIcon;

        private void OnEnable()
        {
            var currentAbilityData = AbilityDataSystem.Instance.GetAbilityData(LevelSceneController.Instance.Player.WeaponController.CurrentAbility.Type);

            currentAbilityName.text = currentAbilityData.Name;
            currentAbilityDescription.text = currentAbilityData.Description;
            currentAbilityIcon.sprite = currentAbilityData.Icon;

            var nextAbilityData = AbilityDataSystem.Instance.GetAbilityData(NextAbilityType);

            nextAbilityName.text = nextAbilityData.Name;
            nextAbilityDescription.text = nextAbilityData.Description;
            nextAbilityIcon.sprite = nextAbilityData.Icon;

            LevelSceneController.Instance.PauseGame();
        }

        private void OnDisable()
        {
            NextAbilityType = AbilityType.None;

            LevelSceneController.Instance.ResumeGame();
        }

        public void OnYesButtonClicked()
        {
            LevelSceneController.Instance.Player.WeaponController.SwitchAbility(NextAbilityType);
            var msg = "You switched to " + NextAbilityType.ToString() + " ability.";
            LevelSceneController.Instance.playerHUD.ShowPickupFeedback(msg);
            LevelSceneController.Instance.playerHUD.ShowAbilityHUD(NextAbilityType);
            Hide();
        }
    }
}