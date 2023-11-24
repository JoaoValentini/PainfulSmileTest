using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Gameplay
{
    public class GameCanvas : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _playerScoreTxt;
        [SerializeField] TextMeshProUGUI _sessionTimerTxt;
        [SerializeField] TextMeshProUGUI _countdownTxt;
        [SerializeField] GameObject _countdownPanel;
        
        [Header("Endgame")]
        [SerializeField] GameObject _endgamePanel;
        [SerializeField] TextMeshProUGUI _endgameScoreTxt;

        void Awake()
        {
            _countdownPanel.SetActive(true);
            _endgamePanel.SetActive(false);
        }

        void Start()
        {
            GameSession.Instance.onGameStarted += OnGameStart;
            GameSession.Instance.onGameStartCountdown += OnGameStartCountdown;
            GameSession.Instance.onGameEnded += OnGameEnded;
            GameSession.Instance.onPlayerScoreChanged += OnPlayerScoreChanged;
            
            GameSession.Instance.onSessionTimerUpdated += UpdateTimeRemainingText;
        }

        void OnGameStartCountdown(int time)
        {
            if(time <= 0)
            {
                _countdownTxt.text = "Start!";
                return;
            }
            _countdownTxt.text = time.ToString();

        }
        void OnGameStart()
        {
            _countdownPanel.SetActive(false);
        }

        void OnPlayerScoreChanged(int score)
        {
            _playerScoreTxt.text = score.ToString();
        }

        void UpdateTimeRemainingText(TimeSpan timeRemaining)
        {
            if (timeRemaining.TotalSeconds >= 10)
                _sessionTimerTxt.text = timeRemaining.ToString(@"mm\:ss");
            else
                _sessionTimerTxt.text = timeRemaining.ToString(@"ss\:ff");

        }

        void OnGameEnded()
        {
            StartCoroutine(OnGameEndedRoutine());
        }
        
        IEnumerator OnGameEndedRoutine()
        {
            yield return new WaitForSeconds(1);
            _endgamePanel.SetActive(true);
            _endgameScoreTxt.text = $"Final Score: {GameSession.Instance.PlayerScore}";
        }

        public void OnClickPlayAgain()
        {
            Manager.Instance.GoToGame();
        }
        public void OnClickMainMenu()
        {
            Manager.Instance.GoToMainMenu();
        }
    }
}
