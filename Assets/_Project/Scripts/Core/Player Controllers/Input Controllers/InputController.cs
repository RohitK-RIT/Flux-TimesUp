using System;
using _Project.Scripts.Core.Character;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers.Input_Controllers
{
    public abstract class InputController : CharacterComponent
    {
        public abstract event Action<Vector2> OnMoveInputUpdated;
        public abstract event Action OnAttackInputBegan, OnAttackInputEnded;
        public virtual event Action OnReloadInput;
        public virtual event Action OnAbilityEquipped;
        public virtual event Action<int> OnSwitchWeaponInput;
    }
}