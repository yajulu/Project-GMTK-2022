using Core;
using DG.Tweening;
using UI;

namespace Enemy
{
    public class EnemyMainControllerBase : MainControllerBase
    {
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
            gameObject.SetActive(false);
            if (gameObject.transform.parent.name.Contains("EnemyHolder"))
            {
                var parent = gameObject.transform.parent;
                parent.DOComplete();
                parent.SetParent(null);
                Destroy(parent.gameObject, 3f);
            }
            else
                Destroy(gameObject, 5f);
        }
    }
}
