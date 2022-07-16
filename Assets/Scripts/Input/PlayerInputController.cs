using System;
using UnityEngine;
using Yajulu.Input;

namespace Input
{
    public class PlayerInputController : MonoBehaviour
    {
        private Main mainInput;
        private void OnEnable()
        {
            mainInput.Enable();
        }

        private void OnDisable()
        {
            mainInput.Disable();
        }

        void Awake()
        {
            mainInput = new Main();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public Vector2 GetPlayerInput()
        {
            return mainInput.Player.Move.ReadValue<Vector2>();
        }
    }
}
