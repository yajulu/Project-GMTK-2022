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

        public PlayerDataDictionary PlayerDict => playerDict;

        protected override void Start()
        {
            base.Start();
            SwitchPlayerType(currentPlayerType);
        }

        [Button]
        private void SwitchPlayerType(ePlayerType type)
        {
            playerDict[currentPlayerType].Graphics.SetActive(false);
            currentPlayerType = type;
            playerDict[currentPlayerType].Graphics.SetActive(true);
            PlayerTypeChanged?.Invoke(currentPlayerType);
        }
        
    }
}
