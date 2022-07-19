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

        public GameObject CurrentPlayerVariantGraphics => playerDict[currentPlayerType].Graphics;

        private GameObject currentPlayerVariantGraphics;

        protected override void Awake()
        {
            base.Awake();
            diceManager = FindObjectOfType<DiceManager>();
            diceManager.PlayerDiceRolled += SwitchPlayerType;
            // damageScale = Vector3.one * 0.1f;
        }

        protected override void Start()
        {
            base.Start();
            //SwitchPlayerType(currentPlayerType);
        }

        private void OnDestroy()
        {
            diceManager.PlayerDiceRolled -= SwitchPlayerType;
        }

        [Button]
        public void SwitchPlayerType(ePlayerType type)
        {
            foreach (var properties in playerDict.Values)
            {
                properties.VariantObject.SetActive(false);
            }
            currentPlayerType = type;
            playerDict[currentPlayerType].VariantObject.SetActive(true);
            PlayerTypeChanged?.Invoke(currentPlayerType);
        }

    }
}
