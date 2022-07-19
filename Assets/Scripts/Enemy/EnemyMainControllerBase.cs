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
        [SerializeField, TitleGroup("Refs")] private GameObject deadGraphics;
        [SerializeField, TitleGroup("Refs")] private GameObject weaponTransform; 
        [SerializeField, TitleGroup("Refs")] private Collider2D enemyCollider;


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
            gfx.transform.gameObject.SetActive(false);
            deadGraphics.SetActive(true);
            enemyCollider.enabled = false;
            if (!gameObject.transform.parent.SafeIsUnityNull()  && gameObject.transform.parent.name.Contains("EnemyHolder"))
            {
                var parent = gameObject.transform.parent;
                parent.DOComplete();
                parent.SetParent(null);
                Destroy(parent.gameObject, 3f);
            }
            else
                Destroy(gameObject, 5f);
        }

        protected override void SetRefs()
        {
            base.SetRefs();
            deadGraphics = transform.FindDeepChild<GameObject>("DeadGFX");
            weaponTransform = transform.FindDeepChild<GameObject>("EnemyWeaponBase");
            enemyCollider = GetComponent<Collider2D>();
        }
    }
}
