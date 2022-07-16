using Sirenix.OdinInspector;
using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(menuName = "Weapon/NewWeapon")]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField, MinValue(1)] private int damage;

        public int Damage => damage;
    }
}
