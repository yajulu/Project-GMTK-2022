using System;
using Input;
using Player.Abilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayerMotor : MonoBehaviour
    {
        private PlayerInputController inputController;
        private PlayerDashAbility dashAbility;
        
        [SerializeField, MinValue(0)] private float movementSpeed = 10f;
        [SerializeField, ReadOnly, TitleGroup("Debug")] private bool movementPause;

        private Vector2 currentMoveVector;
        private void Awake()
        {
            inputController = GetComponent<PlayerInputController>();
            dashAbility = GetComponent<PlayerDashAbility>();
        }

        private void OnEnable()
        {
            dashAbility.DashPerformed += SetPlayerMovementPause;
            movementPause = false;
        }

        private void OnDisable()
        {
            dashAbility.DashPerformed -= SetPlayerMovementPause;
        }
        
        void Start()
        {

        }

        void Update()
        {
            HandlePlayerMovement();
        }
        
        private void SetPlayerMovementPause(bool pause)
        {
            movementPause = pause;
        }


        private void HandlePlayerMovement()
        {
            if (movementPause)
                return;
            inputController.GetPlayerInput(out currentMoveVector);
            transform.Translate(currentMoveVector * (movementSpeed * Time.deltaTime));
        }

        [Button]
        private void SetRefs()
        {
            // _inputController = GetComponent<PlayerInputController>();
        }
    }
}
