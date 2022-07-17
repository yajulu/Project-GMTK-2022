using Core;

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
            gameObject.SetActive(false);
            transform.SetParent(null);
            Destroy(gameObject, 5f);
        }
    }
}
