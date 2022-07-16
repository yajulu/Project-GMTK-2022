using System;
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

        void Awake()
        {
            mainInput = new Main();
        }
        
        private void OnEnable()
        {
            mainInput.Player.Enable();
            mainCamera = Camera.main;
            if (mainCamera != null) mainCameraTransform = mainCamera.transform;
        }
        
        private void OnDisable()
        {
            mainInput.Player.Disable();
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
