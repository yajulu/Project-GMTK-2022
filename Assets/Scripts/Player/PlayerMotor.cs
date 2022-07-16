using System;
using Input;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayerMotor : MonoBehaviour
    {
        private PlayerInputController inputController;
        [SerializeField, MinValue(0)] private float movementSpeed = 10f;

        private Vector2 currentMoveVector;
        private void Awake()
        {
            inputController = GetComponent<PlayerInputController>();
        }

        void Start()
        {

        }

        void Update()
        {
            HandlePlayerMovement();
        }

        private void HandlePlayerMovement()
        {
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
