using System;
using System.Collections.Generic;
using Essentials;
using Input;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Yajulu.Input;
using Random = UnityEngine.Random;
using UI;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Core
{
    public class DiceManager : MonoBehaviour
    {

        [SerializeField] private float userCoolDownTime = 3f;
        [SerializeField] private List<eEnemyType> enemyTypes;
        [SerializeField] private List<ePlayerType> playerTypes;

        [SerializeField, ReadOnly] private eEnemyType currentEnemyType;
        [SerializeField, ReadOnly] private ePlayerType currentPlayerType;

        [SerializeField, ReadOnly] private float aiDiceRollTimer;
        [SerializeField, ReadOnly] private float coolDownTimer;

        public event Action<eEnemyType> EnemyDiceRolled;
        public event Action<ePlayerType> PlayerDiceRolled;

        public UnityEvent enemyDiceRolledUnityEvent;
        public UnityEvent playerDiceRolledUnityEvent;
        
        public UnityEvent userDiceRolledAnimationStartedUnityEvent;
        public UnityEvent aiDiceRolledAnimationStartedUnityEvent;
        
        public UnityEvent userDiceRolledAnimationCompletedUnityEvent;
        public UnityEvent aiDiceRolledAnimationCompletedUnityEvent;

        [SerializeField] private AnimatorEventTrigger userAnimatorTrigger;
        [SerializeField] private AnimatorEventTrigger aiDiceAnimatorTrigger;
        
        

        private List<eDiceType> playingDices;

        private Main mainInput;

        private PlayerMainControllerBase _playerMainControllerBase;

        // private const string ENEMY_DICE = "EnemyDice";
        // private const string PLAYER_DICE = "PlayerDice";
        // private const string PLAYER_ENEMEY_DICE = "PlayerEnemyDice";

        private int ENEMY_DICE_HASH;
        private int PLAYER_DICE_HASH;
        private int PLAYER_ENEMEY_DICE_HASH;

        [SerializeField, TitleGroup("Debug")] private EnumAnimatorHashesDict<ePlayerType> playerTypeHashes;
        [SerializeField, TitleGroup("Debug")] private EnumAnimatorHashesDict<eEnemyType> enemyTypeHashes;
        [SerializeField, TitleGroup("Debug")] private EnumAnimatorHashesDict<eDiceType> diceTriggerHashes;
        [SerializeField, TitleGroup("Debug")] private EnumAnimatorHashesDict<eDiceType> diceIntegerHashes;

        [SerializeField] private bool userRolling;
        [SerializeField] private bool aiRolling;

        private void Awake()
        {
            playingDices = new List<eDiceType>();
        }

        private void Start()
        {
            _playerMainControllerBase = FindObjectOfType<PlayerMainControllerBase>();
            mainInput = _playerMainControllerBase.GetComponent<PlayerInputController>().MainInput;
            mainInput.Player.RollEnemyDice.performed += RollEnemyDiceOnPerformed;
            mainInput.Player.RollPlayerDice.performed += RollPlayerDiceOnPerformed;
            _playerMainControllerBase.PlayerTypeChanged += UpdateCurrentPlayerType;
            // ENEMY_DICE_HASH = Animator.StringToHash(nameof(eEnemyType));
            // PLAYER_DICE_HASH = Animator.StringToHash(nameof(ePlayerType));
            // PLAYER_ENEMEY_DICE_HASH = Animator.StringToHash(PLAYER_ENEMEY_DICE);

        }

        private void OnDisable()
        {
            if (!mainInput.IsUnityNull())
            {
                mainInput.Player.RollEnemyDice.performed -= RollEnemyDiceOnPerformed;
                mainInput.Player.RollPlayerDice.performed -= RollPlayerDiceOnPerformed;
            }
            if (!_playerMainControllerBase.IsUnityNull())
                _playerMainControllerBase.PlayerTypeChanged -= UpdateCurrentPlayerType;
        }

        private void RollPlayerDiceOnPerformed(InputAction.CallbackContext obj)
        {
            RollDice(eDiceType.Player, true);
        }

        private void RollEnemyDiceOnPerformed(InputAction.CallbackContext obj)
        {
            RollDice(eDiceType.Enemy, true);
        }

        private void Update()
        {
            if (UIManager.Instance.CurrentGameState != UIManager.GameState.Started)
                return;
            if (aiDiceRollTimer < 0)
            {
                aiDiceRollTimer = Random.Range(5, 10);
                RollDice(Random.Range(0, 4) > 1 ? eDiceType.Enemy : eDiceType.Player, false);
                return;
            }
            aiDiceRollTimer -= Time.deltaTime;
            coolDownTimer -= Time.deltaTime;

            UIManager.Instance.UpdateCooldown(coolDownTimer, userCoolDownTime);

        }

        private void UpdateCurrentPlayerType(ePlayerType type)
        {
            currentPlayerType = type;
        }

        [Button]
        public void RollUserDiceOverride<T>(T type) where T : Enum
        {
            int stateHash;
            if (type is ePlayerType playerType)
            {
                userAnimatorTrigger.AddStateCallBack(diceTriggerHashes[eDiceType.Player], () =>
                {
                    var localType = playerType;
                    OnPlayerDiceRolled(localType);
                });
                userAnimatorTrigger.AttachedAnimator.SetInteger(diceIntegerHashes[eDiceType.Player], (int)playerType);
                userAnimatorTrigger.AttachedAnimator.SetTrigger(diceTriggerHashes[eDiceType.Player]);
            }
            else if (type is eEnemyType enemyType)
            {
                userAnimatorTrigger.AddStateCallBack(diceTriggerHashes[eDiceType.Enemy], () =>
                {
                    var localType = enemyType;
                    OnEnemyDiceRolled(localType);
                });
                userAnimatorTrigger.AttachedAnimator.SetInteger(diceIntegerHashes[eDiceType.Enemy], (int)enemyType);
                userAnimatorTrigger.AttachedAnimator.SetTrigger(diceTriggerHashes[eDiceType.Enemy]);
            }

        }

        [Button]
        private void RollDice(eDiceType diceType, bool user, bool immediate = false)
        {
            
            switch (user)
            {
                case true when ((coolDownTimer > 0 && UIManager.Instance.CurrentGameState == UIManager.GameState.Started) || userRolling):
                case false when aiRolling:
                    return;
            }

            AnimatorEventTrigger currentAnimator;
            if (user)
            {
                currentAnimator = userAnimatorTrigger;
                userRolling = true;
                coolDownTimer = userCoolDownTime;
                userDiceRolledAnimationStartedUnityEvent?.Invoke();
            }
            else
            {
                currentAnimator = aiDiceAnimatorTrigger;
                aiRolling = true;
                aiDiceRolledAnimationStartedUnityEvent?.Invoke();
            }

            Action callBack;
            int newStateHash;

            switch (diceType)
            {
                case eDiceType.Player:
                    ePlayerType newPlayerType = GetRandomType(playerTypes, currentPlayerType);
                    newStateHash = playerTypeHashes[newPlayerType];
                    currentAnimator.AttachedAnimator.SetInteger(diceIntegerHashes[diceType], (int)newPlayerType);
                    callBack = () =>
                    {
                        var localAnimator = currentAnimator;
                        var localDice = diceType;
                        OnPlayerDiceRolled(newPlayerType);
                        if (user)
                        {
                            userRolling = false;
                            userDiceRolledAnimationCompletedUnityEvent?.Invoke();
                        }
                        else
                        {
                            aiRolling = false;
                            aiDiceRolledAnimationCompletedUnityEvent?.Invoke();
                        }
                            
                    };
                    break;
                case eDiceType.Enemy:
                    eEnemyType newEnemyType = GetRandomType(enemyTypes, currentEnemyType);
                    newStateHash = enemyTypeHashes[newEnemyType];
                    currentAnimator.AttachedAnimator.SetInteger(diceIntegerHashes[diceType], (int)newEnemyType);
                    callBack = () =>
                    {
                        var localAnimator = currentAnimator;
                        var localDice = diceType;
                        OnEnemyDiceRolled(newEnemyType);
                        if (user)
                        {
                            userRolling = false;
                            userDiceRolledAnimationCompletedUnityEvent?.Invoke();
                        }
                        else
                        {
                            aiRolling = false;
                            aiDiceRolledAnimationCompletedUnityEvent?.Invoke();
                        }
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(diceType), diceType, null);
            }


            currentAnimator.AddStateCallBack(newStateHash, callBack);
            currentAnimator.AttachedAnimator.SetTrigger(diceTriggerHashes[diceType]);

            // switch (diceType)
            // {
            //     case eDiceType.User_Player:
            //         userAnimatorTrigger.AddStateCallBack(PLAYER_DICE_HASH, () =>
            //         {
            //             OnPlayerDiceRolled(playerTypes[GetRandomIndex(playerTypes, currentPlayerType)]);
            //             playingDices.Remove(diceType);
            //         });
            //         break;
            //     case eDiceType.User_Enemy:
            //         userAnimatorTrigger.AddStateCallBack(ENEMY_DICE_HASH, () =>
            //         {
            //             OnEnemyDiceRolled(enemyTypes[GetRandomIndex(enemyTypes, currentEnemyType)]);
            //             playingDices.Remove(diceType);
            //         });
            //         break;
            //     case eDiceType.AI_Random:
            //         if (Random.Range(0, 3) > 1)
            //         {
            //             aiDiceAnimatorTrigger.AddStateCallBack(PLAYER_DICE_HASH, () =>
            //             {
            //                 OnPlayerDiceRolled(playerTypes[GetRandomIndex(playerTypes, currentPlayerType)]);
            //                 playingDices.Remove(diceType);
            //             });
            //         }
            //         else
            //         {
            //             aiDiceAnimatorTrigger.AddStateCallBack(ENEMY_DICE_HASH, () =>
            //             {
            //                 OnEnemyDiceRolled(enemyTypes[GetRandomIndex(enemyTypes, currentEnemyType)]);
            //                 playingDices.Remove(diceType);
            //             });
            //         }
            //         break;
            //     case eDiceType.AI_PlayerEnemy:
            //
            //         aiDiceAnimatorTrigger.AddStateCallBack(PLAYER_ENEMEY_DICE_HASH, () =>
            //         {
            //             OnEnemyDiceRolled(enemyTypes[GetRandomIndex(enemyTypes, currentEnemyType)]);
            //             OnPlayerDiceRolled(playerTypes[GetRandomIndex(playerTypes, currentPlayerType)]);
            //             playingDices.Remove(diceType);
            //         });
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException(nameof(diceType), diceType, null);
            // }

            int GetRandomIndex<T>(IReadOnlyList<T> list, T current)
            {
                int index;
                index = Random.Range(0, list.Count);
                index = list[index].Equals(current)
                    ? (index + 1) % list.Count : index;
                return index;
            }

            T GetRandomType<T>(IReadOnlyList<T> list, T current)
            {
                int index;
                index = Random.Range(0, list.Count);
                index = list[index].Equals(current)
                    ? (index + 1) % list.Count : index;
                return list[index];
            }

        }

        protected virtual void OnEnemyDiceRolled(eEnemyType enemyType)
        {
            currentEnemyType = enemyType;
            Debug.Log($"$Enemy Dice Rolled - {enemyType}");
            EnemyDiceRolled?.Invoke(enemyType);
            enemyDiceRolledUnityEvent?.Invoke();
        }


        protected virtual void OnPlayerDiceRolled(ePlayerType playerType)
        {
            currentPlayerType = playerType;
            Debug.Log($"$Enemy Dice Rolled - {playerType}");
            PlayerDiceRolled?.Invoke(playerType);
            playerDiceRolledUnityEvent?.Invoke();
        }


        [Button]
        private void PrepareAnimationHashes()
        {
            playerTypeHashes = new EnumAnimatorHashesDict<ePlayerType>();
            foreach (var type in Enum.GetValues(typeof(ePlayerType)))
            {
                playerTypeHashes.Add((ePlayerType)type, Animator.StringToHash(type.ToString()));
            }

            enemyTypeHashes = new EnumAnimatorHashesDict<eEnemyType>();
            foreach (var type in Enum.GetValues(typeof(eEnemyType)))
            {
                enemyTypeHashes.Add((eEnemyType)type, Animator.StringToHash(type.ToString()));
            }

            diceTriggerHashes = new EnumAnimatorHashesDict<eDiceType>();
            foreach (var type in Enum.GetValues(typeof(eDiceType)))
            {
                diceTriggerHashes.Add((eDiceType)type, Animator.StringToHash(type.ToString()));
            }

            diceIntegerHashes = new EnumAnimatorHashesDict<eDiceType>
            {
                { eDiceType.Player, Animator.StringToHash(nameof(ePlayerType)) },
                { eDiceType.Enemy, Animator.StringToHash(nameof(eEnemyType)) }
            };
        }

        [Serializable]
        public class EnumAnimatorHashesDict<T> : UnitySerializedDictionary<T, int>
        {

        }

    }
}
