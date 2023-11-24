using System.Collections;
using System.Collections.Generic;
using Gameplay.Ships;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class PlayerShip : Ship
    {   
        [SerializeField] PlayerCombat _combat;
        [SerializeField] PlayerMovement _movement;
        [SerializeField] PlayerInput _playerInput;
        
        [Space]
        [SerializeField] float _maxHealth;
        
        internal PlayerCombat Combat => _combat;
        internal PlayerMovement Movement => _movement;

        void Awake()
        {
            InitializeShip(_maxHealth);
            
            _combat.Initialize(this);
            _movement.Initialize(this);

            _playerInput.enabled = false;
        }
        
        void Start()
        {
            GameSession.Instance.onGameStarted += OnGameStart;
            GameSession.Instance.onGameEnded += OnGameEnded;
        }

        void OnGameStart()
        {
            _playerInput.enabled = true;
        }
        void OnGameEnded()
        {
            _playerInput.enabled = false;
            ShipCanvas.HideHealthBar();
        }

        public override void DestroyShip()
        {
            GameSession.Instance.EndGame();
            base.DestroyShip();
        }
    }
}
