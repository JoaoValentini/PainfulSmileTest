using System.Collections;
using System.Collections.Generic;
using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        Transform[] _spawnPoints;
        [SerializeField] EnemyShip[] _enemyShipsPrefabs;

        void Awake()
        {
            SetSpawnPoints();
        }

        void Start()
        {
            GameSession.Instance.onGameStarted += OnGameStarted;
            GameSession.Instance.onGameEnded += OnGameEnded;
        }

        void SetSpawnPoints()
        {
            int spawnPointsCount = transform.childCount;
            _spawnPoints = new Transform[spawnPointsCount];
            for (int i = 0; i < spawnPointsCount; i++)
            {
                _spawnPoints[i] = transform.GetChild(i);
            }
        }

        void OnGameStarted()
        {
            StartCoroutine(SpawnEnemiesRoutine());
        }
        void OnGameEnded()
        {
            StopAllCoroutines();
        }

        IEnumerator SpawnEnemiesRoutine()
        {
            var wait = new WaitForSeconds(Manager.Instance.GameOptions.EnemySpawnTimeSeconds);
            while(true)
            {
                SpawnRandomEnemy();
                yield return wait;
            }
        }

        void SpawnRandomEnemy()
        {
            int spawnPositionIndex = Random.Range(0, _spawnPoints.Length);
            int enemyPrefabIndex = Random.Range(0, _enemyShipsPrefabs.Length);

            if(_enemyShipsPrefabs[enemyPrefabIndex] == null)
                return;

            EnemyShip enemy = Instantiate(_enemyShipsPrefabs[enemyPrefabIndex], _spawnPoints[spawnPositionIndex].position, Quaternion.identity);
            enemy.LookAt(GameSession.Instance.Player.Position);
        }
    }
}
