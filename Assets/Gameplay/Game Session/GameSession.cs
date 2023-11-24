using System;
using System.Collections;
using System.Diagnostics;
using Gameplay.Enemies;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay
{
    public class GameSession : MonoBehaviour
    {
        public static GameSession Instance {get; private set;}

        [SerializeField] PlayerShip _player;
        public PlayerShip Player => _player;

        [SerializeField] Camera _mainCamera;
        public Camera MainCamera => _mainCamera;
        
        [SerializeField] LayerMask _playerLayerMask;
        [SerializeField] LayerMask _enemiesLayerMask;
        [SerializeField] LayerMask _landLayerMask;
        public LayerMask PlayerLayerMask => _playerLayerMask;
        public LayerMask EnemiesLayerMask => _enemiesLayerMask;
        public LayerMask LandLayerMask => _landLayerMask;

        [SerializeField] int _countDownTime = 3;
        public event Action onGameStarted;
        public event Action<int> onGameStartCountdown;
        public event Action onGameEnded;

        public Action<EnemyShip> onEnemyDefeated;
        public event Action<int> onPlayerScoreChanged;
        
        int _playerScore = 0;
        public int PlayerScore => _playerScore;
        
        Stopwatch _sessionStopwatch = new Stopwatch();
        TimeSpan _sessionTotalTime;
        public event Action<TimeSpan> onSessionTimerUpdated;

        void Awake()
        {
            Instance = this;
            onEnemyDefeated += OnEnemyDefeated;
            SetTimer();
        }
        
        void Start()
        {
            StartCoroutine(StartGameRoutine());
        }

        void SetTimer()
        {
            int gameTimeMiliseconds = Manager.Instance.GameOptions.GameSessionTimeSeconds * 1000;
            _sessionTotalTime = new TimeSpan(0,0,Manager.Instance.GameOptions.GameSessionTimeSeconds);
        }

        IEnumerator StartGameRoutine()
        {
            yield return new WaitForEndOfFrame();

            int currentCountdownTime = _countDownTime;
            onGameStartCountdown?.Invoke(currentCountdownTime);

            while (currentCountdownTime >= 0)
            {
                yield return new WaitForSeconds(1);
                currentCountdownTime--;
                onGameStartCountdown?.Invoke(currentCountdownTime);
            }

            _sessionStopwatch.Start();
            onGameStarted?.Invoke();

            while (_sessionStopwatch.IsRunning)
            {
                onSessionTimerUpdated?.Invoke(_sessionTotalTime - _sessionStopwatch.Elapsed);
                if(_sessionStopwatch.Elapsed >= _sessionTotalTime)
                    EndGame();
                
                yield return null;
            }
        }

        
        void OnEnemyDefeated(EnemyShip enemyShip)
        {
            if(!_sessionStopwatch.IsRunning)
                return;
                
            _playerScore++;
            onPlayerScoreChanged?.Invoke(_playerScore);
        }

        public void EndGame()
        {
            _sessionStopwatch.Stop();
            onGameEnded?.Invoke();
        }
    }
}
