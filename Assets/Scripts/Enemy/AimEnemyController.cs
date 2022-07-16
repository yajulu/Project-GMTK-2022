using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Weapons;

namespace Enemy
{
    public class AimEnemyController : MonoBehaviour
    {

        #region Static Properties

        private static Transform Player
        {
            get
            {
                if (_player.SafeIsUnityNull())
                {
                    _player = GameObject.FindWithTag("Player").transform;
                }
                return _player;
            }
            
        }

        private static Transform _player;
        
        private static readonly Vector3 UpVector = Vector3.forward;

        #endregion

        [SerializeField, TitleGroup("Properties")]
        private float rotationSpeed;
        [SerializeField, TitleGroup("Refs")] private Transform weaponTransform;

        private Transform playerInstance;

        // private float dummyDeltaAngle;
        private Vector3 dummyCrossProduct;
        private void Awake()
        {
            playerInstance = Player;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            AimWeapon();
        }

        private void AimWeapon()
        {
            // dummyDeltaAngle =
            //     Vector3.SignedAngle(weaponTransform.right, Player.position - transform.position, UpVector);
            dummyCrossProduct = Vector3.Cross((transform.position - playerInstance.position), weaponTransform.right);
            weaponTransform.Rotate(UpVector, dummyCrossProduct.z * rotationSpeed);
        }

        [Button]
        private void SetRefs()
        {
            weaponTransform = GetComponentInChildren<WeaponControllerBase>().transform;
        }
    }
}
