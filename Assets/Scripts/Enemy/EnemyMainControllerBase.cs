using Core;
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
            transform.SetParent(null);
            Destroy(gameObject, 5f);
        }
    }
}
