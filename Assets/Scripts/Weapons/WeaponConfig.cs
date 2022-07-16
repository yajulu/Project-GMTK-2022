using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Weapons
{
    [Serializable]
    public class WeaponConfig 
    {
        [SerializeField, MinValue(1)] private int damage;

        public int Damage => damage;
    }
}
