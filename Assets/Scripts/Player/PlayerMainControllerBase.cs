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
        public void SwitchPlayerType(ePlayerType type)
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
