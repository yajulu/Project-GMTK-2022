using Input;
using UnityEngine;

namespace Player.Abilities
{
    public class PlayerAbilityBase : MonoBehaviour
    {
        protected PlayerInputController InputController;

        protected Vector2 CurrentMousePosition;
        protected Vector2 CurrentAimDirection;
        
        protected virtual void Awake()
        {
            InputController = GetComponent<PlayerInputController>();
        }
        
        protected void UpdateCurrentAimDirection(Vector2 refPosition)
        {
            InputController.GetPlayerWorldMousePosition(out CurrentMousePosition);
            CurrentAimDirection = CurrentMousePosition - (Vector2) refPosition;
        }
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
