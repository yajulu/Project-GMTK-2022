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
        private Vector2 currentMousePosition;
        private Vector2 currentAimDirection;
        
        private readonly Vector3 upVector = Vector3.back;
        protected void Awake()
        {
            weapon = GetComponentInChildren<WeaponControllerBase>();
            weaponTransform = weapon.transform;
            inputController = GetComponent<PlayerInputController>();
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
            inputController.GetPlayerWorldMousePosition(out currentMousePosition);
            currentAimDirection = currentMousePosition - (Vector2) weaponTransform.position;
            weaponTransform.right = currentAimDirection;
        }
    }
}
