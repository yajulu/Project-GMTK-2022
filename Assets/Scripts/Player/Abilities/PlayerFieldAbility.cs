using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Abilities
{
    public class PlayerFieldAbility : PlayerAbility
    {
        [SerializeField] private Transform fieldTransform;

        private Tweener fieldGraphicsTween;
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

        protected override void OnAbilityPerformed(InputAction.CallbackContext obj)
        {
            if (fieldTransform.gameObject.activeSelf)
                return;
            // SetAbilityField(true);
            fieldTransform.DOScale(1f, 0.25f)
                .From(0)
                .SetEase(Ease.OutBack)
                .OnStart(Completed);

            void Completed()
            {
                base.OnAbilityPerformed(obj);
            }
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
