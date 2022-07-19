using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using DG.Tweening;
using Essentials;
using Sirenix.OdinInspector;
using UI;
using Unity.VisualScripting;
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
        [SerializeField, TitleGroup("Spawning")] private int maxNumber = 4;
        [SerializeField, TitleGroup("Spawning")] private Vector2 spawnRange;
        [SerializeField, TitleGroup("Spawning")]
        private SpawnQuadrant[] quadList;

        [SerializeField, TitleGroup("Progression")]
        private int maxProgressionSpawnValue;

        [SerializeField, TitleGroup("Progression")]
        private float incrementsTime;
        
        [SerializeField, TitleGroup("Debug")] private eEnemyType currentEnemyType;
        [SerializeField, TitleGroup("Debug"), ReadOnly] private float spawnTimer;
        [SerializeField, TitleGroup("Debug"), ReadOnly] private float progressionIncrementTimer;
        [SerializeField, TitleGroup("Debug"), ReadOnly] private int nextQuadIndex;
        [SerializeField, TitleGroup("Debug"), ReadOnly] private int spawnCounter;
        //[SerializeField, TitleGroup("Debug"), ReadOnly] private int spawnCounter;


        private GameObject dummyCurrentEnemy;

        private DiceManager diceManager;

        private void Awake()
        {
            diceManager = FindObjectOfType<DiceManager>();
            diceManager.EnemyDiceRolled += SwitchEnemies;
            spawning = false;
            progressionIncrementTimer = incrementsTime;
        }

        private void OnEnable()
        {
            spawning = false;
            UIManager.GameStarted += UIManagerOnGameStarted;
        }

        private void OnDisable()
        {
            UIManager.GameStarted -= UIManagerOnGameStarted;
        }

        private void UIManagerOnGameStarted()
        {
            spawning = true;

        }

        private void OnDestroy()
        {
            diceManager.EnemyDiceRolled -= SwitchEnemies;
        }

        private void Start()
        {
            //spawning = true;
        }

        private void Update()
        {
            if (spawning)
            {
                if (spawnTimer < 0 || transform.childCount < maxNumber)
                {
                    SpawnEnemy();
                }

                if (maxNumber < maxProgressionSpawnValue && progressionIncrementTimer < 0)
                {
                    maxNumber++;
                    progressionIncrementTimer = incrementsTime;
                }
                
                    

                spawnTimer -= Time.deltaTime;
                progressionIncrementTimer -= Time.deltaTime;
            }
        }

        private void SpawnEnemy()
        {
            if (transform.childCount >= maxNumber)
                return;
            spawnTimer = spawnInterval;
            var spawnPosition = spawnCounter < maxNumber ? quadList[spawnCounter % quadList.Length].GetRandomPositionInQuadrant() : GetRandomPosition();
            var initialPosition = spawnPosition.normalized * 20;
            var config = enemyTypeDict[currentEnemyType];
            // dummyCurrentEnemy = null;
            var newEnemy = Instantiate(config.Prefab, initialPosition, config.RandomizeRotation ? Quaternion.Euler(0, 0, Random.Range(0, 360)) : Quaternion.identity, transform);
            newEnemy.transform.DOMove(spawnPosition, 3f);
            // Debug.Log($"Spawning {newEnemy.name} at {spawnPosition}");
            spawnCounter++;

        }


        [Button]
        private void SwitchEnemies(eEnemyType newType)
        {
            currentEnemyType = newType;
            var count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var currentChild = transform.GetChild(i);
                var config = enemyTypeDict[currentEnemyType];
                Instantiate(config.Prefab, currentChild.position, config.RandomizeRotation ? Quaternion.Euler(0, 0, Random.Range(0, 360)) : Quaternion.identity, transform);
                currentChild.gameObject.SetActive(false);
                Destroy(currentChild.gameObject, 3f);
            }
        }

        private Vector2 GetRandomPosition()
        {
            var selectedQuad = quadList.DefaultIfEmpty(quadList[spawnCounter % quadList.Length]).FirstOrDefault(quadrant =>
            {

                for (int i = 0; i < transform.childCount; i++)
                {
                    var enemy = transform.GetChild(i);
                    if (quadrant.IsPositionInsideQuadrant(enemy.position))
                        return false;
                }

                return true;
            });

            selectedQuad = selectedQuad.IsUnityNull() ? quadList[Random.Range(0, quadList.Length)] : selectedQuad;
            return selectedQuad.GetRandomPositionInQuadrant();
        }

        [Serializable]
        public class SpawnQuadrant
        {
            public Vector2 center;
            [OnValueChanged(nameof(UpdateRange), true)] public Vector2 range;

            [SerializeField, ReadOnly]
            private Vector2 halfRange;

            private Vector2 tempVal;

            private void UpdateRange()
            {
                halfRange = range * 0.5f;
            }
            public Vector2 GetRandomPositionInQuadrant()
            {
                return new Vector2
                {
                    x = Random.Range(-halfRange.x, halfRange.x) + center.x,
                    y = Random.Range(-halfRange.y, halfRange.y) + center.y
                };
            }

            public bool IsPositionInsideQuadrant(Vector2 position)
            {
                tempVal = position - center;
                return tempVal.x > -halfRange.x && tempVal.x < halfRange.x && tempVal.y > -halfRange.y && tempVal.y < halfRange.y;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach (var quadrant in quadList)
            {
                Gizmos.DrawWireCube(quadrant.center, quadrant.range);
            }
        }
    }

    [Serializable]
    public class EnemyTypeDict : UnitySerializedDictionary<eEnemyType, EnemySpawnConfig>
    {

    }

    [Serializable]
    public class EnemySpawnConfig
    {
        public GameObject Prefab;
        public bool RandomizeRotation;
        public Vector2 InitialSpawnPosition;
    }

}
