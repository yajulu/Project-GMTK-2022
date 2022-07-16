using System;
using UnityEngine;

namespace Player.Abilities
{
    public class PlayerDashAbility : PlayerAbility
    {
        protected override void Start()
        {
            base.Start();
            AbilityAction = InputController.MainInput.Player.Dash;
            AbilityAction.performed += OnAbilityPerformed;
        }

        private void OnDestroy()
        {
            AbilityAction.performed -= OnAbilityPerformed;
        }
    }
}
