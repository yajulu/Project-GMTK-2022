using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.Abilities
{
    public class PlayerDashAbility : PlayerAbility
    {

        [SerializeField] private Transform dashTransform;

        protected override void OnEnable()
        {
            base.OnEnable();
            dashTransform.gameObject.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            AbilityAction = InputController.MainInput.Player.Dash;
            AbilityAction.performed += OnAbilityPerformed;
            AbilityPerformed += SetDashAbilityGraphics;
        }

        private void SetDashAbilityGraphics(bool enable)
        {
            dashTransform.gameObject.SetActive(enable);
            dashTransform.right = CurrentAimDirection;
        }

        private void OnDisable()
        {
            if (!AbilityAction.IsUnityNull())
                AbilityAction.performed -= OnAbilityPerformed;
            AbilityPerformed -= SetDashAbilityGraphics;
        }
    }
}
