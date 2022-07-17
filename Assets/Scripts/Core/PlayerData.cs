
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
        
        [SerializeField] private GameObject graphics;

        public float MovementSpeed => movementSpeed;

        public int MaxHealthPoints => maxHealthPoints;

        public GameObject Graphics => graphics;
    }

    [Serializable]
    public class PlayerDataDictionary : UnitySerializedDictionary<ePlayerType, PlayerProperties>
    {
        
    }
}
