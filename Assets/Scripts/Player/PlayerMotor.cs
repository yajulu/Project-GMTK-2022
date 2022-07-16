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
        private PlayerInputController _inputController;
        [SerializeField, MinValue(0)] private float movementSpeed = 10f;

        private void Awake()
        {
            _inputController = GetComponent<PlayerInputController>();
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
            transform.Translate(_inputController.GetPlayerInput() * (movementSpeed * Time.deltaTime));
        }

        [Button]
        private void SetRefs()
        {
            // _inputController = GetComponent<PlayerInputController>();
        }
    }
}
