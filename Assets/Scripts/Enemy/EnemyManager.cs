using System;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using Essentials;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        public event Action<eEnemyType> EnemySwitched;

        [SerializeField, TitleGroup("Spawning")] private EnemyTypeDict enemyTypeDict;
        [SerializeField, TitleGroup("Spawning")] private bool spawning = true;
        [SerializeField, TitleGroup("Spawning")] private float spawnInterval = 15f;
        [SerializeField, TitleGroup("Spawning")] private Vector2 spawnRange;
        
        [SerializeField, TitleGroup("Debug")] private eEnemyType currentEnemyType;
        [SerializeField, TitleGroup("Debug"), ReadOnly] private float spawnTimer;

        private GameObject dummyCurrentEnemy;
        
        private void Start()
        {
            spawning = true;
        }

        private void Update()
        {
            if (spawning)
            {
                if (spawnTimer < 0)
                {
                    SpawnEnemy();
                }

                spawnTimer -= Time.deltaTime;
            }
        }

        private void SpawnEnemy()
        {
            spawnTimer = spawnInterval;
            var spawnPosition = GetRandomPosition();
            var initialPosition = new Vector2(Mathf.Abs(spawnPosition.x) + spawnRange.x * 0.5f,
                Mathf.Abs(spawnPosition.y) + spawnRange.y * 0.5f);
            dummyCurrentEnemy = Instantiate(enemyTypeDict[currentEnemyType], spawnPosition, Quaternion.identity, transform);
            dummyCurrentEnemy.transform.DOMove(spawnPosition, 3f)
                .From(initialPosition);
        }

        private void SwitchEnemies(eEnemyType newType)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var currentChild = transform.GetChild(i);

                Instantiate(enemyTypeDict[currentEnemyType], currentChild.position, Quaternion.identity, transform);
            }
        }

        private Vector2 GetRandomPosition()
        {
            return new Vector2(Random.Range(-spawnRange.x, spawnRange.x), Random.Range(-spawnRange.y, spawnRange.y));
        }
    }

    [Serializable]
    public class EnemyTypeDict : UnitySerializedDictionary<eEnemyType, GameObject>
    {
        
    }

}