
using System;
using Essentials;
using UnityEngine;

namespace Core
{
    public class PlayerConfig : ScriptableObject
    {
        
    }

    [Serializable]
    public class PlayerProperties
    {
        [SerializeField] private float movementSpeed;
        
        [SerializeField] private int maxHealthPoints;
        
        [SerializeField] private Transform gfx;
    }

    [Serializable]
    public class PlayerDataDictionary : UnitySerializedDictionary<ePlayerType, PlayerProperties>
    {
        
    }
}
