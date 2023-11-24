using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameOptions
{
    [SerializeField] int _gameSessionTimeSeconds = 90;
    [SerializeField] int _minGameSessionTime = 60;
    [SerializeField] int _maxGameSessionTime = 180;
    [Space]
    [SerializeField] int _enemySpawnTimeSeconds = 5;
    [SerializeField] int _minEnemySpawnTime = 1;
    [SerializeField] int _maxEnemySpawnTime = 10;
    
    
    public int GameSessionTimeSeconds => _gameSessionTimeSeconds;
    public int MinGameSessionTime => _minGameSessionTime;
    public int MaxGameSessionTime => _maxGameSessionTime;
    public int EnemySpawnTimeSeconds => _enemySpawnTimeSeconds;
    public int MinEnemySpawnTime => _minEnemySpawnTime;
    public int MaxEnemySpawnTime => _maxEnemySpawnTime;

    public void SetEnemySpawnTime(int seconds)
    {
        _enemySpawnTimeSeconds = Mathf.Clamp(seconds, _minEnemySpawnTime, _maxEnemySpawnTime);
    }
    
    public void SetGameSessionTime(int seconds)
    {
        _gameSessionTimeSeconds = Mathf.Clamp(seconds, _minGameSessionTime, _maxGameSessionTime);
    }
}
