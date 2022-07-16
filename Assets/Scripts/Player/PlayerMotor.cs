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
        private PlayerAbility ability;
        
        [SerializeField, MinValue(0)] private float movementSpeed = 10f;
        [SerializeField, ReadOnly, TitleGroup("Debug")] private bool movementPause;

        private Vector2 currentMoveVector;
        private void Awake()
        {
            inputController = GetComponent<PlayerInputController>();
            ability = GetComponent<PlayerAbility>();
        }

        private void OnEnable()
        {
            ability.AbilityPerformed += SetPlayerMovementPause;
            movementPause = false;
        }

        private void OnDisable()
        {
            ability.AbilityPerformed -= SetPlayerMovementPause;
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
