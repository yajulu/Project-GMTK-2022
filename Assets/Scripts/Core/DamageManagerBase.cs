using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class DamageManagerBase : MonoBehaviour, IDamageable
    {
        [SerializeField] protected int maxHealthPoints = 100;
        [SerializeField, ReadOnly, TitleGroup("Debug")] protected int currentHealthPoint;
        [SerializeField, ReadOnly, TitleGroup("Debug")] protected bool isDead;

        public event Action<int> OnDamageTaken;
        public event Action OnDamageableKilled;

        public UnityEvent OnDamageableKilledUnityEvent;

        protected bool IsDead => isDead;
        public int MaxHealthPoints => maxHealthPoints;

        protected virtual void OnEnable()
        {
            InitDamageable();
        }

        protected virtual void OnDisable()
        {
            
        }

        protected virtual void InitDamageable()
        {
            currentHealthPoint = maxHealthPoints;
            isDead = false;
        }
        public virtual void TakeDamage(int dmg)
        {
            if (isDead || !enabled)
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
            OnDamageableKilledUnityEvent?.Invoke();
            OnDamageableKilled?.Invoke();
        }
        
    }
}
