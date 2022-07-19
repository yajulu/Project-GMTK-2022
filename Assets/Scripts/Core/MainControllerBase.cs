using System;
using DG.Tweening;
using Essentials;
using Sirenix.OdinInspector;
using UnityEngine;
using Weapons;

namespace Core
{
    public class MainControllerBase : MonoBehaviour
    {
        [SerializeField, ReadOnly, TitleGroup("Refs")] protected DamageManagerBase damageManager;

        [SerializeField, ReadOnly, TitleGroup("Refs")]
        protected Transform gfx;

        [SerializeField] protected Vector3 damageScale;

        public Transform MainGraphicsHolder => gfx;

        protected virtual void Awake()
        {
            // damageScale = Vector3.one * 0.1f;
        }

        protected virtual void OnEnable()
        {
            damageManager.OnDamageTaken += DamageManagerOnDamageTaken;
        }

        protected virtual void OnDisable()
        {
            damageManager.OnDamageTaken -= DamageManagerOnDamageTaken;
        }

        protected virtual void Start()
        {
            
        }

        void Update()
        {
        
        }
        
        private void DamageManagerOnDamageTaken(int obj)
        {
            gfx.DOComplete();
            gfx.DOPunchScale(damageScale, 0.2f);
        }

        [Button]
        protected virtual void SetRefs()
        {
            damageManager = GetComponent<DamageManagerBase>();
            gfx = transform.FindDeepChild<Transform>("GFX");
        }
        
    }
}
