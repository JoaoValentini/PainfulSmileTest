using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Menu
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] Slider _gameTimeSlider;
        [SerializeField] TextMeshProUGUI _gameTimeValueTxt;
        [SerializeField] Slider _enemySpawnTimeSlider;
        [SerializeField] TextMeshProUGUI _enemySpawnTimeValueTxt;

        internal void UpdateUI()
        {
            GameOptions options = Manager.Instance.GameOptions;

            _gameTimeSlider.minValue = options.MinGameSessionTime;
            _gameTimeSlider.maxValue = options.MaxGameSessionTime;
            _gameTimeSlider.SetValueWithoutNotify(options.GameSessionTimeSeconds);
            _gameTimeValueTxt.text = $"{options.GameSessionTimeSeconds} seconds";
            
            _enemySpawnTimeSlider.minValue = options.MinEnemySpawnTime;
            _enemySpawnTimeSlider.maxValue = options.MaxEnemySpawnTime;
            _enemySpawnTimeSlider.SetValueWithoutNotify(options.EnemySpawnTimeSeconds);
            _enemySpawnTimeValueTxt.text = $"{options.EnemySpawnTimeSeconds} seconds";
        }

        public void ChangeGameSessionTime(float value)
        {
            GameOptions options = Manager.Instance.GameOptions;
            options.SetGameSessionTime((int)value);
            _gameTimeValueTxt.text = $"{options.GameSessionTimeSeconds} seconds";
        }

        public void ChangeEnemySpawnTime(float value)
        {
            GameOptions options = Manager.Instance.GameOptions;
            options.SetEnemySpawnTime((int)value);
            _enemySpawnTimeValueTxt.text = $"{options.EnemySpawnTimeSeconds} seconds";
        }
    }
}
