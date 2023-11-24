using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Ships
{
    public class ShipCanvas : MonoBehaviour
    {
        Ship _ship;
        [SerializeField] GameObject _canvasPanel;
        [SerializeField] Image _healthBar;
        [SerializeField] Image _lostHealthBar;
        [SerializeField] Color _healthBarColor;
        [SerializeField] Color _lostHealthBarColor;
        float _lostHealthBarShowTime = 0.5f;
        float _lostHealthBarHideSpeed = 2f;

        void Awake()
        {
            _healthBar.fillAmount = 1;
            _lostHealthBar.fillAmount = 1;
            _healthBar.color = _healthBarColor;
            _lostHealthBar.color = _lostHealthBarColor;
        }

        internal void Initialize(Ship ship)
        {
            _ship = ship;
            _ship.onDamaged += OnShipDamaged;
            _ship.onDestroyed += OnShipDestroyed;
        }

        void OnShipDamaged(float currentHealth, float maxHealth)
        {
            float fill = currentHealth / maxHealth;
            _healthBar.fillAmount = fill;
            StopAllCoroutines();
            StartCoroutine(HealthBarAnimationRoutine());
        }

        void OnShipDestroyed()
        {
            HideHealthBar();
        }

        IEnumerator HealthBarAnimationRoutine()
        {
            yield return new WaitForSeconds(_lostHealthBarShowTime);

            while(_lostHealthBar.fillAmount > _healthBar.fillAmount)
            {
                _lostHealthBar.fillAmount -= Time.deltaTime * _lostHealthBarHideSpeed;
                yield return null;
            }
        }

        public void ShowHealthBar()
        {
            _canvasPanel.SetActive(true);
        }
        public void HideHealthBar()
        {
            _canvasPanel.SetActive(false);
        }
    }
}
