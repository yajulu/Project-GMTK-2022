using System;
using UnityEngine;

namespace Player
{
    public class PlayerMainController : MonoBehaviour, IDamageable
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void TakeDamage(int dmg)
        {
            Debug.Log(dmg);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            Debug.Log(col);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("Trigger");
        }
    }
}
