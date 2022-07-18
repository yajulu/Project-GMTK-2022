using System;
using UnityEngine;
using Core;
using Player.Abilities;
using Sirenix.OdinInspector;
using UI;

namespace Player
{
    public class PlayerDamageManagerBase : DamageManagerBase
    {
        [SerializeField, ReadOnly, TitleGroup("Debug")] protected bool isInvulnerable;
        private PlayerDashAbility dashAbility;
        private PlayerFieldAbility fieldAbility;
        private PlayerMainControllerBase mainController;

        private void Awake()
        {
            dashAbility = GetComponent<PlayerDashAbility>();
            fieldAbility = GetComponent<PlayerFieldAbility>();
            mainController = GetComponent<PlayerMainControllerBase>();

        }

        protected override void OnEnable()
        {
            base.OnEnable();
            dashAbility.AbilityPerformed += SetInvulnerability;
            fieldAbility.AbilityPerformed += SetInvulnerability;
            mainController.PlayerTypeChanged += MainControllerOnPlayerTypeChanged;
            OnDamageableKilled += OnOnDamageableKilled;
        }

        private void OnOnDamageableKilled()
        {
            UIManager.Instance.StopGame();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            dashAbility.AbilityPerformed -= SetInvulnerability;
            fieldAbility.AbilityPerformed -= SetInvulnerability;
            mainController.PlayerTypeChanged -= MainControllerOnPlayerTypeChanged;
            OnDamageableKilled -= OnOnDamageableKilled;
        }

        public override void TakeDamage(int dmg)
        {
            if (isInvulnerable)
                return;

            base.TakeDamage(dmg);
            UIManager.Instance.UpdateHP(currentHealthPoint);

        }

        private void SetInvulnerability(bool enable)
        {
            isInvulnerable = enable;
        }

        private void MainControllerOnPlayerTypeChanged(ePlayerType type)
        {
            maxHealthPoints = mainController.PlayerDict[type].MaxHealthPoints;
            InitDamageable();
        }
    }
}
