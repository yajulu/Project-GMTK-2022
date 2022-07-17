using System;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class PlayerMainControllerBase : MainControllerBase
    {
        [SerializeField] private ePlayerType currentPlayerType;
        
        [SerializeField] private PlayerDataDictionary playerDict;
        public event Action<ePlayerType> PlayerTypeChanged;

        private DiceManager diceManager;
        public PlayerDataDictionary PlayerDict => playerDict;

        protected override void Awake()
        {
            base.Awake();
            diceManager = FindObjectOfType<DiceManager>();
            diceManager.PlayerDiceRolled += SwitchPlayerType;
        }
        
        protected override void Start()
        {
            base.Start();
            SwitchPlayerType(currentPlayerType);
        }

        private void OnDestroy()
        {
            diceManager.PlayerDiceRolled -= SwitchPlayerType;
        }

        [Button]
        private void SwitchPlayerType(ePlayerType type)
        {
            foreach (var properties in playerDict.Values)
            {
                properties.Graphics.SetActive(false);
            }
            currentPlayerType = type;
            playerDict[currentPlayerType].Graphics.SetActive(true);
            PlayerTypeChanged?.Invoke(currentPlayerType);
        }
        
    }
}
