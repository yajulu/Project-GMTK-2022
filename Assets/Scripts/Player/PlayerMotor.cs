using System;
using Core;
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
        private PlayerMainControllerBase mainController;
        
        [SerializeField, MinValue(0)] private float movementSpeed = 10f;
        [SerializeField, ReadOnly, TitleGroup("Debug")] private bool movementPause;

        private Vector2 currentMoveVector;
        private void Awake()
        {
            inputController = GetComponent<PlayerInputController>();
            dashAbility = GetComponent<PlayerDashAbility>();
            mainController = GetComponent<PlayerMainControllerBase>();
        }

        private void OnEnable()
        {
            dashAbility.AbilityPerformed += SetPlayerMovementPause;
            mainController.PlayerTypeChanged += MainControllerOnPlayerTypeChanged;
            movementPause = false;
        }

   
        private void OnDisable()
        {
            dashAbility.AbilityPerformed -= SetPlayerMovementPause;
            mainController.PlayerTypeChanged -= MainControllerOnPlayerTypeChanged;
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
        
        private void MainControllerOnPlayerTypeChanged(ePlayerType type)
        {
            movementSpeed = mainController.PlayerDict[type].MovementSpeed;
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
