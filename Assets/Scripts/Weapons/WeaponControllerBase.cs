using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(ParticleSystem))]
    public class WeaponControllerBase : MonoBehaviour
    {
        [SerializeField, TitleGroup("Properties")]
        private WeaponConfig weaponConfig;
        [SerializeField, FoldoutGroup("Refs")] private ParticleSystem part; 
        private List<ParticleCollisionEvent> collisionEvents;
        private IDamageable dummyDamageable;

        protected virtual void Start()
        {
            collisionEvents = new List<ParticleCollisionEvent>();
        }

        private void OnParticleCollision(GameObject other)
        {
            var numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

            if (numCollisionEvents <= 0 || !other.TryGetComponent<IDamageable>(out dummyDamageable)) return;
            foreach (var collisionEvent in collisionEvents)
            {
                dummyDamageable.TakeDamage(weaponConfig.Damage);
            }
        }

        [Button]
        protected virtual void SetRefs()
        {
            part = GetComponent<ParticleSystem>();
        }
    }
}
