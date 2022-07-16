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
        private PlayerAbility ability;

        private void Awake()
        {
            ability = GetComponent<PlayerAbility>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ability.AbilityPerformed += SetInvulnerability;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ability.AbilityPerformed -= SetInvulnerability;
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
