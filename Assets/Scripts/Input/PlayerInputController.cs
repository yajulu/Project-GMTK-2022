using System;
using Core;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Yajulu.Input;

namespace Input
{
    public class PlayerInputController : MonoBehaviour
    {
        private Main mainInput;

        private Camera mainCamera;
        private Transform mainCameraTransform;
        private Vector3 currentScreenMousePosition;

        public Main MainInput => mainInput;
        private PlayerMainControllerBase mainController;

        void Awake()
        {
            mainInput = new Main();
            mainController = GetComponent<PlayerMainControllerBase>();
        }
        
        private void OnEnable()
        {
            mainInput.Player.Enable();
            mainCamera = Camera.main;
            if (mainCamera != null) mainCameraTransform = mainCamera.transform;
            mainController.PlayerTypeChanged += MainControllerOnPlayerTypeChanged;
            DisableAbilities();
        }
        
        private void OnDisable()
        {
            mainInput.Player.Disable();
            mainController.PlayerTypeChanged -= MainControllerOnPlayerTypeChanged;
        }

        private void MainControllerOnPlayerTypeChanged(ePlayerType type)
        {
            switch (type)
            {
                case ePlayerType.Toktok:
                    mainInput.Player.Dash.Disable();;
                    mainInput.Player.Field.Disable();
                    break;
                case ePlayerType.Bus:
                    mainInput.Player.Dash.Disable();;
                    mainInput.Player.Field.Enable();
                    break;
                case ePlayerType.MotorCycle:
                    mainInput.Player.Dash.Enable();
                    mainInput.Player.Field.Disable();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void DisableAbilities()
        {
            mainInput.Player.Dash.Disable();
            mainInput.Player.Field.Disable();
        }
        
        // Update is called once per frame
        void Update()
        {

        }

        public void GetPlayerInput(out Vector2 move)
        {
            move = mainInput.Player.Move.ReadValue<Vector2>();
        }

        public void GetPlayerWorldMousePosition(out Vector2 position)
        {
            currentScreenMousePosition = mainInput.Player.Aim.ReadValue<Vector2>();
            // currentScreenMousePosition.z = 1000;
            position = mainCamera.ScreenToWorldPoint(currentScreenMousePosition);
        }
        
    }
}
