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
        [SerializeField, OnValueChanged(nameof(UpdateClampValue))] private Vector2 movementRange;
        [SerializeField, ReadOnly, TitleGroup("Debug")] private Vector2 movementRangeHalfExtent;
        [SerializeField, ReadOnly, TitleGroup("Debug")] private bool movementPause;

        private Vector2 currentMoveVector;

        private Vector2 newPosition;
        private Vector2 currentDisplacement;
        private Vector2 currentPosition;
        
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

        private void UpdateClampValue()
        {
            movementRangeHalfExtent = movementRange * 0.5f;
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
            currentPosition = transform.position;
            currentDisplacement = currentMoveVector * (movementSpeed * Time.deltaTime);
            newPosition = currentPosition + currentDisplacement;
            
            newPosition.x = Mathf.Clamp(newPosition.x, -movementRangeHalfExtent.x, movementRangeHalfExtent.x);
            newPosition.y = Mathf.Clamp(newPosition.y, -movementRangeHalfExtent.y, movementRangeHalfExtent.y);

            currentDisplacement = newPosition - currentPosition;
            
            transform.Translate(currentDisplacement);
        }

        [Button]
        private void SetRefs()
        {
            // _inputController = GetComponent<PlayerInputController>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero, movementRange);
        }
    }
}
