using System;
using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace Player.Abilities
{
    public class PlayerDashAbility : PlayerAbilityBase
    {
        [SerializeField, TitleGroup("Properties")]
        private int dashDamage;
        
        [SerializeField, TitleGroup("Properties")]
        private float dashDistance;
        
        [SerializeField, TitleGroup("Properties")]
        private float dashRadius;
        
        [SerializeField, TitleGroup("Properties")]
        private float dashDuration;
        
        [SerializeField, TitleGroup("Properties")]
        private Ease dashEase;
        
        [SerializeField, TitleGroup("Properties")]
        private float coolDown;
        
        [SerializeField, TitleGroup("Properties")]
        private LayerMask damageMask;
        
        [SerializeField, TitleGroup("Properties")]
        private bool  dashMouseEnable;

        [SerializeField, ReadOnly, TitleGroup("Debug")] private bool isCoolingDown;
        [SerializeField, ReadOnly, TitleGroup("Debug")] private bool isDashing;
        [SerializeField, ReadOnly, TitleGroup("Debug")] private float coolDownTimer;

        public event Action<bool> DashPerformed;

        private InputAction dashAction;
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

        private void OnEnable()
        {
            isCoolingDown = false;
            isDashing = false;
        }

        private void OnDisable()
        {
            //Fallback if something went wrong. TO make sure these components are enabled again.
            damageManager.enabled = true;
            motor.enabled = true;
        }

        private void OnDestroy()
        {
            dashAction.performed -= OnDashPerformed;
        }
        
        protected void Start()
        {
            dashAction = InputController.MainInput.Player.Dash;
            dashAction.performed += OnDashPerformed;
        }
        
        void Update()
        {
            UpdateCoolDownTimer();
        }
        
        private void OnDashPerformed(InputAction.CallbackContext obj)
        {
            StartDash();
        }

        private void StartDash()
        {
            if (isCoolingDown || isDashing)
                return;
            isDashing = true;
            if (dashMouseEnable)
                UpdateCurrentAimDirection(transform.position);
            else
                InputController.GetPlayerInput(out CurrentAimDirection);
                
            
            transform.DOBlendableMoveBy(CurrentAimDirection.normalized * dashDistance, dashDuration)
                .SetEase(dashEase)
                .OnStart(DashStart)
                .OnComplete(DashComplete);

            void DashStart()
            {
                ApplyDamageToEnemies();
                isDashing = true;
                DashPerformed?.Invoke(true);
            }

            void DashComplete()
            {
                isDashing = false;
                isCoolingDown = true;
                coolDownTimer = coolDown;
                DashPerformed?.Invoke(false);
            }
        }

        private void ApplyDamageToEnemies()
        {
            currentDetectedEnemiesCount = Physics2D.CircleCastNonAlloc(transform.position, dashRadius,
                CurrentAimDirection, enemiesHit, dashDistance, damageMask);

            for (var i = 0; i < currentDetectedEnemiesCount; i++)
            {
                if (enemiesHit[i].collider.gameObject.TryGetComponent(out dummyDamageable))
                {
                    dummyDamageable.TakeDamage(dashDamage);   
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
            Gizmos.DrawWireSphere(transform.position, dashRadius);
        }
    }
}
