using UnityEngine;

namespace Core
{
    public class DamageableChild : MonoBehaviour , IDamageable
    {
        [SerializeField] private DamageManagerBase damageBase;

        public void TakeDamage(int dmg)
        {
            damageBase.TakeDamage(dmg);
        }
    }
}
