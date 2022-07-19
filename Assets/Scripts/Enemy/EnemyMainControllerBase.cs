using Core;
using DG.Tweening;
using Essentials;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UI;
using UnityEngine;

namespace Enemy
{
    public class EnemyMainControllerBase : MainControllerBase
    {
        [SerializeField, TitleGroup("Refs")] private Animator enemyAnimator;
        [SerializeField, TitleGroup("Refs")] private GameObject weaponTransform; 
        [SerializeField, TitleGroup("Refs")] private Collider2D enemyCollider;

        private int deadTriggerHash;

        private AnimatorOverrideController animatorOverrideController;

        protected override void Awake()
        {
            base.Awake();
            enemyAnimator = GetComponentInChildren<Animator>();
            deadTriggerHash = Animator.StringToHash("Dead");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            damageManager.OnDamageableKilled += DamageManagerOnOnDamageableKilled;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            damageManager.OnDamageableKilled -= DamageManagerOnOnDamageableKilled;
        }

        private void DamageManagerOnOnDamageableKilled()
        {
            UIManager.Instance.UpdatePlayerScore(1000);
            weaponTransform.SetActive(false);
            // gfx.transform.gameObject.SetActive(false);
            enemyAnimator.SetTrigger(deadTriggerHash);
            enemyCollider.enabled = false;
            if (!gameObject.transform.parent.SafeIsUnityNull()  && gameObject.transform.parent.name.Contains("EnemyHolder"))
            {
                var parent = gameObject.transform.parent;
                parent.DOComplete();
                parent.SetParent(null);
                Destroy(parent.gameObject, 1f);
            }
            else
                Destroy(gameObject, 1f);
        }

        protected override void SetRefs()
        {
            base.SetRefs();
            enemyAnimator = GetComponentInChildren<Animator>();
            weaponTransform = transform.FindDeepChild<GameObject>("EnemyWeaponBase");
            enemyCollider = GetComponent<Collider2D>();
        }
    }
}
