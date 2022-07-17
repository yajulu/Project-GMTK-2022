using System;
using UnityEngine;
using Core;
using Player.Abilities;
using Sirenix.OdinInspector;

namespace Player
{
    public class PlayerDamageManagerBase : DamageManagerBase
    {
        [SerializeField, ReadOnly, TitleGroup("Debug")] protected bool isInvulnerable;
        private PlayerDashAbility dashAbility;
        private PlayerFieldAbility fieldAbility;

        private void Awake()
        {
            dashAbility = GetComponent<PlayerDashAbility>();
            fieldAbility = GetComponent<PlayerFieldAbility>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            dashAbility.AbilityPerformed += SetInvulnerability;
            fieldAbility.AbilityPerformed += SetInvulnerability;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            dashAbility.AbilityPerformed -= SetInvulnerability;
            fieldAbility.AbilityPerformed -= SetInvulnerability;
        }

        public override void TakeDamage(int dmg)
        {
            if (isInvulnerable)
                return;
            base.TakeDamage(dmg);
        }

        private void SetInvulnerability(bool enable)
        {
            isInvulnerable = enable;
        }
        
    }
}
