using System;
using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Weapons;

namespace Player.Abilities
{
    public class PlayerAbility : PlayerAbilityBase
    {
        [SerializeField, TitleGroup("Properties")]
        private int damage;
        
        [SerializeField, TitleGroup("Properties")]
        private float distance;
        
        [SerializeField, TitleGroup("Properties")]
        private float radius;
        
        [SerializeField, TitleGroup("Properties")]
        private float duration;
        
        [SerializeField, TitleGroup("Properties")]
        private Ease ease;
        
        [SerializeField, TitleGroup("Properties")]
        private float coolDown;
        
        [SerializeField, TitleGroup("Properties")]
        private LayerMask damageMask;
        
        [SerializeField, TitleGroup("Properties")]
        private bool  dashMouseEnable;

        [SerializeField, ReadOnly, TitleGroup("Debug")] private bool isCoolingDown;
        [SerializeField, ReadOnly, TitleGroup("Debug")] private bool isPerformingAbility;
        [SerializeField, ReadOnly, TitleGroup("Debug")] private float coolDownTimer;

        public event Action<bool> AbilityPerformed;
        public UnityEvent AbilityPreformedUnityEvent;

        protected InputAction AbilityAction;
        private PlayerMotor motor;
        private DamageManagerBase damageManager;

        private RaycastHit2D[] enemiesHit;
        private int currentDetectedEnemiesCount;

        private IDamageable dummyDamageable;

        protected override void Awake()
        {
            base.Awake();
            motor = GetComponent<PlayerMotor>();
            damageManager = GetComponent<DamageManagerBase>();
            enemiesHit = new RaycastHit2D [10];
        }

        protected virtual void OnEnable()
        {
            isCoolingDown = false;
            isPerformingAbility = false;
        }

        protected virtual void Start()
        {
            
        }
        void Update()
        {
            UpdateCoolDownTimer();
        }
        
        protected virtual void OnAbilityPerformed(InputAction.CallbackContext obj)
        {
            StartAbility();
        }

        private void StartAbility()
        {
            if (isCoolingDown || isPerformingAbility)
                return;
            isPerformingAbility = true;
            if (dashMouseEnable)
                UpdateCurrentAimDirection(transform.position);
            else
                InputController.GetPlayerInput(out CurrentAimDirection);
                
            
            transform.DOBlendableMoveBy(CurrentAimDirection.normalized * distance, duration)
                .SetEase(ease)
                .OnStart(DashStart)
                .OnComplete(DashComplete);

            void DashStart()
            {
                ApplyDamageToEnemies();
                isPerformingAbility = true;
                AbilityPerformed?.Invoke(true);
                AbilityPreformedUnityEvent?.Invoke();
            }

            void DashComplete()
            {
                isPerformingAbility = false;
                isCoolingDown = true;
                coolDownTimer = coolDown;
                AbilityPerformed?.Invoke(false);
            }
        }

        private void ApplyDamageToEnemies()
        {
            currentDetectedEnemiesCount = Physics2D.CircleCastNonAlloc(transform.position, radius,
                CurrentAimDirection, enemiesHit, distance, damageMask);

            for (var i = 0; i < currentDetectedEnemiesCount; i++)
            {
                if (enemiesHit[i].collider.gameObject.TryGetComponent(out dummyDamageable))
                {
                    dummyDamageable.TakeDamage(damage);   
                }
            }
            
        }

        private void UpdateCoolDownTimer()
        {
            if (!isCoolingDown)
                return;
            coolDownTimer -= Time.deltaTime;
            isCoolingDown = coolDownTimer > 0;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
