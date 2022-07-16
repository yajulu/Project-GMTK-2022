using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Weapons
{
    public class DamageManagerBase : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxHealthPoints = 100;
        [SerializeField, ReadOnly, TitleGroup("Debug")] protected int currentHealthPoint;
        [SerializeField, ReadOnly, TitleGroup("Debug")] protected bool isDead;

        public event Action<int> OnDamageTaken;
        public event Action OnDamageableKilled;

        protected bool IsDead => isDead;

        public int MaxHealthPoints => maxHealthPoints;

        private void OnEnable()
        {
            InitDamageable();
        }
        protected virtual void InitDamageable()
        {
            currentHealthPoint = maxHealthPoints;
            isDead = false;
        }
        public virtual void TakeDamage(int dmg)
        {
            if (isDead)
                return;
            currentHealthPoint -= dmg;
            Debug.Log($"{gameObject.name} took {dmg} damage points, current HP {currentHealthPoint}");
            OnDamageTaken?.Invoke(dmg);
            
            //Killed
            if (currentHealthPoint < 0 && !isDead)
            {
                KillObject();
            }
        }

        protected virtual void KillObject()
        {
            isDead = true;
            Debug.Log($"{gameObject.name} is Dead.");
            OnDamageableKilled?.Invoke();
        }
        
    }
}
