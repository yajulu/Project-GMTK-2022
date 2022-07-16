using System;
using Input;
using Sirenix.OdinInspector;
using UnityEngine;
using Weapons;

namespace Player.Abilities
{
    public class PlayerAimAbility : PlayerAbilityBase
    {
        private WeaponControllerBase weapon;
        private PlayerInputController inputController;

        private Transform weaponTransform;

        private readonly Vector3 upVector = Vector3.back;
        protected override void Awake()
        {
            base.Awake();
            weapon = GetComponentInChildren<WeaponControllerBase>(true);
            weaponTransform = weapon.transform;
        }

        private void OnEnable()
        {
            weaponTransform.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            weaponTransform.gameObject.SetActive(false);
        }

        protected void Start()
        {
        
        }

        protected void Update()
        {
            AimWeapon();
        }
        
        private void AimWeapon()
        {
            UpdateCurrentAimDirection(weaponTransform.position);
            weaponTransform.right = CurrentAimDirection;
        }
    }
}
