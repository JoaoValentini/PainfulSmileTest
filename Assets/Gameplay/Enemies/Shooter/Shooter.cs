using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class Shooter : EnemyShip
    {
        [SerializeField] ShooterData _shooterData;
        protected override EnemyData enemyData { get => _shooterData;}

        enum ShooterState {Idle, Repositioning, Shooting, Recharging}
        ShooterState _currentState = ShooterState.Idle;

        // Repositioning
        int _maxRespositioningAttempts = 20;
        float _pathTimeout = 5f;
        float _currentPathTime = 0;

        //Shooting
        [SerializeField] Projectile _projectilePrefab;
        [SerializeField] Transform _projectileSpawnPoint;
        float _rechargeTime = 0;
        int _shotCounter = 0;
        float _firstShotTimer;
        float _nextShotTimer;


        new void Start()
        {
            base.Start();
            ChangeState(ShooterState.Repositioning);
        }

        #region Repositioning

        void RepositionUpdate()
        {
            _currentPathTime += Time.deltaTime;
            if(_currentPathTime >= _pathTimeout)
            {
                OnEnterRepositionState();
                return;
            }
            
            SetDestination(_destination);
            MoveToDestination();

            if(IsInShootingDistanceAndVisibility())
                ChangeState(ShooterState.Shooting);
        }

        void OnEnterRepositionState()
        {
            Vector3 playerPos = GameSession.Instance.Player.Position;
            Vector3 playerEnemyDirection = (Position - playerPos).normalized;
            bool newDestination = GetNewShootingPosition(playerEnemyDirection, playerPos, out Vector3 newPosition, 1);
            
            if(!newDestination)
            {
                ChangeState(ShooterState.Recharging);
                return;
            }

            _currentPathTime = 0;
            _destination = newPosition;
            SetDestination(_destination);
            _updateDelegate = RepositionUpdate;
        }

        bool GetNewShootingPosition(Vector3 playerEnemyDirecion, Vector3 playerPos, out Vector3 position, int attemptNumber)
        {
            if(attemptNumber > _maxRespositioningAttempts)
            {
                position = Vector3.zero;
                return false;
            }
            float halfAngle = _shooterData.MaxRepositioningAngle /2f;
            float randomAngle = Random.Range(-halfAngle, halfAngle);
            float radiusOffset = Random.Range(-_shooterData.DistanceToMoveFromPlayerThreshold, _shooterData.DistanceToMoveFromPlayerThreshold);
            
            Vector3 randomPointDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * playerEnemyDirecion;
            position = randomPointDirection * (radiusOffset + _shooterData.DistanceToMoveFromPlayer) + playerPos;

            if(!GameUtility.IsInCameraView(position, GameSession.Instance.MainCamera))
                return GetNewShootingPosition(playerEnemyDirecion, playerPos, out position, ++attemptNumber);
            
            if(!GameUtility.IsInSight(playerPos, position, GameSession.Instance.LandLayerMask))
                return GetNewShootingPosition(playerEnemyDirecion, playerPos, out position, ++attemptNumber);

            return true;
        }

        protected override void OnReachDestination()
        {
            if (_currentState != ShooterState.Repositioning)
                return;
            if (IsInShootingDistanceAndVisibility())
                ChangeState(ShooterState.Shooting);
            else
                OnEnterRepositionState();
        }

        #endregion

        #region Shooting

        void OnEnterShootingState()
        {
            _firstShotTimer = 0;
            _nextShotTimer = 0;
            _updateDelegate = ShootingUpdate;
        }

        void ShootingUpdate()
        {
            LookTowards(GameSession.Instance.Player.Position);

            if(_firstShotTimer < _shooterData.FirstShotDelay)
            {
                _firstShotTimer += Time.deltaTime;
                return;
            }

            if(_nextShotTimer > Time.time)
                return;

            _nextShotTimer = Time.time + _shooterData.DelayBetweenShots;
            
            Projectile projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, Quaternion.identity);
            projectile.Shoot(_projectileSpawnPoint.up * _shooterData.ProjectileSpeed, _shooterData.Damage, GameSession.Instance.PlayerLayerMask);
            _shotCounter++;

            if(_shotCounter >= _shooterData.ShotsPerCharge)
            {
                ChangeState(ShooterState.Recharging);
            }
        }

        #endregion

        #region Recharging

        void OnEnterRechargingState()
        {
            _shotCounter = 0;
            _rechargeTime = 0;
            _updateDelegate = RechargingUpdate;
        }

        void RechargingUpdate()
        {
            _rechargeTime += Time.deltaTime;

            if(_rechargeTime >= _shooterData.RechargeTimeSeconds)
            {
                if(IsInShootingDistanceAndVisibility())
                    ChangeState(ShooterState.Shooting);
                else
                    ChangeState(ShooterState.Repositioning);
            }
        }

        #endregion

        void ChangeState(ShooterState newState)
        {
            if(newState == _currentState)
                return;

            _currentState = newState;
            switch (_currentState)
            {
                case ShooterState.Repositioning:
                    OnEnterRepositionState();
                return;
                case ShooterState.Shooting:
                    OnEnterShootingState();
                return;
                case ShooterState.Recharging:
                    OnEnterRechargingState();
                return;
                default: 
                    _updateDelegate = null;
                return;
            }
        }

        protected override void OnGameEnded()
        {
            base.OnGameEnded();
            ChangeState(ShooterState.Idle);
        }

        bool IsInShootingDistanceAndVisibility()
        {
            float distance = Vector3.Distance(GameSession.Instance.Player.Position, Position);
            if(distance > _shooterData.MaxShootingDistance)
                return false;

            return GameUtility.IsInSight(Position, GameSession.Instance.Player.Position, GameSession.Instance.LandLayerMask);
        }
    }
}
