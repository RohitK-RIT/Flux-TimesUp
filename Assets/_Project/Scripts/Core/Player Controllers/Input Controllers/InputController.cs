using System;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers.Input_Controllers
{
    public abstract class InputController
    {
        public abstract event Action<Vector2> OnMoveInputUpdated;
        public abstract event Action OnAttackInputBegan, OnAttackInputEnded;
    }
}