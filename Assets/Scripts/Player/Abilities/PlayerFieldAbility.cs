using UnityEngine;

namespace Player.Abilities
{
    public class PlayerFieldAbility : PlayerAbility
    {
        protected override void Start()
        {
            base.Start();
            AbilityAction = InputController.MainInput.Player.Field;
            AbilityAction.performed += OnAbilityPerformed;
        }

        private void OnDestroy()
        {
            AbilityAction.performed -= OnAbilityPerformed;
        }
    }
}
