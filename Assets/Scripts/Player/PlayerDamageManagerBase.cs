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

        private void Awake()
        {
            dashAbility = GetComponent<PlayerDashAbility>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            dashAbility.DashPerformed += SetInvulnerability;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            dashAbility.DashPerformed -= SetInvulnerability;
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
