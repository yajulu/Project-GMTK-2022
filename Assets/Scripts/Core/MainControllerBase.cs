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
        [SerializeField, ReadOnly, FoldoutGroup("Refs")] private DamageManagerBase damageManager;

        [SerializeField, ReadOnly, FoldoutGroup("Refs")]
        private Transform gfx;

        private Vector3 damageScale;

        private void Awake()
        {
            damageScale = Vector3.one * 1.1f;
        }

        private void OnEnable()
        {
            damageManager.OnDamageTaken += DamageManagerOnDamageTaken;
        }

        protected virtual void Start()
        {
            
        }

        void Update()
        {
        
        }
        
        private void DamageManagerOnDamageTaken(int obj)
        {
            gfx.DOPunchScale(damageScale, 0.2f);
        }

        [Button]
        private void SetRefs()
        {
            damageManager = GetComponent<DamageManagerBase>();
            gfx = transform.FindDeepChild<Transform>("GFX");
        }
        
    }
}
