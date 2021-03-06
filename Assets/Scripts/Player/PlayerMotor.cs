using System;
using Core;
using Input;
using Player.Abilities;
using Sirenix.OdinInspector;
using UI;
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
        
        [SerializeField, MinValue(0), OnValueChanged(nameof(UpdateHorseRotation))] private float wheelyAngle = 20f;
        [SerializeField, MinValue(0)] private float movementSpeed = 10f;
        [SerializeField, MinValue(0)] private float rotationSpeed = 10f;
        [SerializeField, OnValueChanged(nameof(UpdateClampValue))] private Vector2 movementRange;
        [SerializeField] private Vector2 movementRangeCenter;
        [SerializeField, ReadOnly, TitleGroup("Debug")] private Vector2 movementRangeHalfExtent;
        [SerializeField, ReadOnly, TitleGroup("Debug")] private bool movementPause;

        private Vector2 currentMoveVector;

        private Vector2 newPosition;
        private Vector2 currentDisplacement;
        private Vector2 currentPosition;
        
        private readonly Quaternion backRotation = Quaternion.Euler(0, 180, 0);
        private readonly Quaternion forwardRotation = Quaternion.identity;
        
        private Quaternion horseRotation = Quaternion.Euler(0, 0, 15);

        private float lastFrameDirection;

        private Transform dummyGraphicsTransform;
        
        private void Awake()
        {
            inputController = GetComponent<PlayerInputController>();
            dashAbility = GetComponent<PlayerDashAbility>();
            mainController = GetComponent<PlayerMainControllerBase>();
            UpdateHorseRotation();
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

        private void UpdateHorseRotation()
        {
            horseRotation = Quaternion.Euler(0, 0, wheelyAngle);
        }
        
        private void MainControllerOnPlayerTypeChanged(ePlayerType type)
        {
            movementSpeed = mainController.PlayerDict[type].MovementSpeed;
        }

        private void HandlePlayerMovement()
        {
            if (movementPause)
                return;

            // if (UIManager.Instance.CurrentGameState != UIManager.GameState.Started &&
            //     UIManager.Instance.CurrentGameState != UIManager.GameState.Tutorial)
            //     return;
            inputController.GetPlayerInput(out currentMoveVector);
            currentPosition = transform.position;
            currentDisplacement = currentMoveVector * (movementSpeed * Time.deltaTime);
            newPosition = currentPosition + currentDisplacement;
            
            newPosition.x = Mathf.Clamp(newPosition.x, -movementRangeHalfExtent.x + movementRangeCenter.x, movementRangeHalfExtent.x + movementRangeCenter.x);
            newPosition.y = Mathf.Clamp(newPosition.y, -movementRangeHalfExtent.y + movementRangeCenter.y, movementRangeHalfExtent.y + movementRangeCenter.y);

            currentDisplacement = newPosition - currentPosition;
            
            transform.Translate(currentDisplacement);
            dummyGraphicsTransform = mainController.CurrentPlayerVariantGraphics.transform;
            if (Mathf.Abs(currentMoveVector.x) > 0)
            {
                mainController.MainGraphicsHolder.transform.rotation =
                    currentMoveVector.x < 0 ? backRotation : forwardRotation;
                if (dummyGraphicsTransform.localRotation.eulerAngles.z < wheelyAngle)
                {
                    // Debug.Log(mainController.CurrentPlayerVariantGraphics.transform.localRotation.eulerAngles.z);
                    dummyGraphicsTransform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime), Space.Self);
                }
                else
                    dummyGraphicsTransform.localRotation = horseRotation;
            }
            else
            {
                if (dummyGraphicsTransform.localRotation.eulerAngles.z > 0f  && dummyGraphicsTransform.localRotation.eulerAngles.z < wheelyAngle)
                    dummyGraphicsTransform.Rotate(Vector3.back * (rotationSpeed * Time.deltaTime), Space.Self);
                else
                    dummyGraphicsTransform.localRotation = forwardRotation;
            }
        }

        [Button]
        private void SetRefs()
        {
            // _inputController = GetComponent<PlayerInputController>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(movementRangeCenter, movementRange);
        }
    }
}
