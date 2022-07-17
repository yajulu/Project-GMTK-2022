using System;
using System.Collections.Generic;
using Essentials;
using Input;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Yajulu.Input;
using Random = UnityEngine.Random;
using UI;

namespace Core
{
    public class DiceManager : MonoBehaviour
    {
        [SerializeField] private float timer;
        [SerializeField] private List<eEnemyType> enemyTypes;
        [SerializeField] private List<ePlayerType> playerTypes;

        [SerializeField, ReadOnly] private eEnemyType currentEnemyType;
        [SerializeField, ReadOnly] private ePlayerType currentPlayerType;

        public event Action<eEnemyType> EnemyDiceRolled;
        public event Action<ePlayerType> PlayerDiceRolled;

        [SerializeField] private AnimatorEventTrigger userAnimatorTrigger;
        [SerializeField] private AnimatorEventTrigger aiDiceAnimatorTrigger;

        private List<eDiceType> playingDices;

        private Main mainInput;

        private const string ENEMY_DICE = "EnemyDice";
        private const string PLAYER_DICE = "PlayerDice";
        private const string PLAYER_ENEMEY_DICE = "PlayerEnemyDice";

        private int ENEMY_DICE_HASH;
        private int PLAYER_DICE_HASH;
        private int PLAYER_ENEMEY_DICE_HASH;

        private void Awake()
        {
            playingDices = new List<eDiceType>();
        }

        private void OnDestroy()
        {
            mainInput.Player.RollEnemyDice.performed -= RollEnemyDiceOnPerformed;
            mainInput.Player.RollPlayerDice.performed -= RollPlayerDiceOnPerformed;
        }

        private void RollPlayerDiceOnPerformed(InputAction.CallbackContext obj)
        {
            RollDice(eDiceType.User_Player);
        }

        private void RollEnemyDiceOnPerformed(InputAction.CallbackContext obj)
        {
            RollDice(eDiceType.User_Enemy);
        }

        private void Start()
        {
            mainInput = FindObjectOfType<PlayerInputController>().MainInput;
            mainInput.Player.RollEnemyDice.performed += RollEnemyDiceOnPerformed;
            mainInput.Player.RollPlayerDice.performed += RollPlayerDiceOnPerformed;
            ENEMY_DICE_HASH = Animator.StringToHash(ENEMY_DICE);
            PLAYER_DICE_HASH = Animator.StringToHash(PLAYER_DICE);
            PLAYER_ENEMEY_DICE_HASH = Animator.StringToHash(PLAYER_ENEMEY_DICE);

        }


        private void Update()
        {
            if (UIManager.Instance.CurrentGameState == UIManager.GameState.Started)
                return;
            if (timer < 0)
            {
                timer = Random.Range(10, 20);
                RollDice(eDiceType.AI_Random);
            }
            timer -= Time.deltaTime;

        }

        [Button]
        private void RollDice(eDiceType diceType, bool immediate = false)
        {
            if (playingDices.Contains(diceType))
                return;
            string animationName;
            playingDices.Add(diceType);
            switch (diceType)
            {
                case eDiceType.User_Player:
                    userAnimatorTrigger.PlayAnimation(PLAYER_DICE_HASH, () =>
                    {
                        OnPlayerDiceRolled(playerTypes[GetRandomIndex(playerTypes, currentPlayerType)]);
                        playingDices.Remove(diceType);
                    });
                    break;
                case eDiceType.User_Enemy:
                    userAnimatorTrigger.PlayAnimation(ENEMY_DICE_HASH, () =>
                    {
                        OnEnemyDiceRolled(enemyTypes[GetRandomIndex(enemyTypes, currentEnemyType)]);
                        playingDices.Remove(diceType);
                    });
                    break;
                case eDiceType.AI_Random:
                    if (Random.Range(0, 3) > 1)
                    {
                        aiDiceAnimatorTrigger.PlayAnimation(PLAYER_DICE_HASH, () =>
                        {
                            OnPlayerDiceRolled(playerTypes[GetRandomIndex(playerTypes, currentPlayerType)]);
                            playingDices.Remove(diceType);
                        });
                    }
                    else
                    {
                        aiDiceAnimatorTrigger.PlayAnimation(ENEMY_DICE_HASH, () =>
                        {
                            OnEnemyDiceRolled(enemyTypes[GetRandomIndex(enemyTypes, currentEnemyType)]);
                            playingDices.Remove(diceType);
                        });
                    }
                    break;
                case eDiceType.AI_PlayerEnemy:

                    aiDiceAnimatorTrigger.PlayAnimation(PLAYER_ENEMEY_DICE_HASH, () =>
                    {
                        OnEnemyDiceRolled(enemyTypes[GetRandomIndex(enemyTypes, currentEnemyType)]);
                        OnPlayerDiceRolled(playerTypes[GetRandomIndex(playerTypes, currentPlayerType)]);
                        playingDices.Remove(diceType);
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(diceType), diceType, null);
            }

            int GetRandomIndex<T>(IReadOnlyList<T> list, T current)
            {
                int index;
                index = Random.Range(0, list.Count);
                index = list[index].Equals(current)
                    ? (index + 1) % list.Count : index;
                return index;
            }

        }

        protected virtual void OnEnemyDiceRolled(eEnemyType enemyType)
        {
            currentEnemyType = enemyType;
            Debug.Log($"$Enemy Dice Rolled - {enemyType}");
            EnemyDiceRolled?.Invoke(enemyType);
        }


        protected virtual void OnPlayerDiceRolled(ePlayerType playerType)
        {
            currentPlayerType = playerType;
            Debug.Log($"$Enemy Dice Rolled - {playerType}");
            PlayerDiceRolled?.Invoke(playerType);
        }

    }
}
