using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Abilities
{
    public class PlayerFieldAbility : PlayerAbility
    {
        [SerializeField] private Transform fieldTransform;

        protected override void OnEnable()
        {
            base.OnEnable();
            fieldTransform.gameObject.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            AbilityAction = InputController.MainInput.Player.Field;
            AbilityAction.performed += OnAbilityPerformed;
            AbilityPerformed += SetAbilityField;
        }

        private void SetAbilityField(bool enable)
        {
            fieldTransform.gameObject.SetActive(enable);
        }

        private void OnDisable()
        {
            if (!AbilityAction.IsUnityNull())
                AbilityAction.performed -= OnAbilityPerformed;
            AbilityPerformed -= SetAbilityField;
        }
    }
}
