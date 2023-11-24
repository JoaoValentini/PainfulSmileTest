using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class PlayerCombat : MonoBehaviour//, IDamageable
    {
        PlayerShip _playerShip;
        
        [Header("Projectile")]
        [SerializeField] Projectile _projectilePrefab;
        [SerializeField] float _singleProjectileDamage = 20;
        [SerializeField] float _tripleProjectileDamage = 15;
        [SerializeField] float _projectileSpeed = 20;
        [SerializeField] float _singleFireRate = 3f;
        [SerializeField] float _tripleFireRate = 1f;
        float _nextSingleFireTime = 0;
        float _nextTripleFireTime = 0;

        [Header("Spawn Points")]
        [SerializeField] Transform _singleFireSpawnPoint;
        [SerializeField] Transform _tripleFireSpawnPoint1;
        [SerializeField] Transform _tripleFireSpawnPoint2;
        [SerializeField] Transform _tripleFireSpawnPoint3;

        internal void Initialize(PlayerShip playerShip)
        {
            _playerShip = playerShip;
        }

        void OnFireSingle(InputValue inputValue)
        {
            if(_nextSingleFireTime > Time.time)
                return;

            _nextSingleFireTime = Time.time + 1f /_singleFireRate;
            
            Projectile projectile = Instantiate(_projectilePrefab, _singleFireSpawnPoint.position, Quaternion.identity);
            projectile.Shoot(_singleFireSpawnPoint.up * _projectileSpeed, _singleProjectileDamage, GameSession.Instance.EnemiesLayerMask);
        }
        
        void OnFireTriple(InputValue inputValue)
        {
            if(_nextTripleFireTime > Time.time)
                return;

            _nextTripleFireTime = Time.time + 1f /_tripleFireRate;

            Projectile projectile1 = Instantiate(_projectilePrefab, _tripleFireSpawnPoint1.position, Quaternion.identity);
            Projectile projectile2 = Instantiate(_projectilePrefab, _tripleFireSpawnPoint2.position, Quaternion.identity);
            Projectile projectile3 = Instantiate(_projectilePrefab, _tripleFireSpawnPoint3.position, Quaternion.identity);
            
            Vector3 mousePos = GameSession.Instance.MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.y = 0;
            Vector3 mouseShipDirection = mousePos - _playerShip.Position;

            float directionDot = Vector3.Dot(mouseShipDirection, _playerShip.GraphicsTransform.right);
            Vector3 fireDirection = directionDot >= 0 ? _playerShip.GraphicsTransform.right : -_playerShip.GraphicsTransform.right;

            projectile1.Shoot(fireDirection * _projectileSpeed, _tripleProjectileDamage, GameSession.Instance.EnemiesLayerMask);
            projectile2.Shoot(fireDirection * _projectileSpeed, _tripleProjectileDamage, GameSession.Instance.EnemiesLayerMask);
            projectile3.Shoot(fireDirection * _projectileSpeed, _tripleProjectileDamage, GameSession.Instance.EnemiesLayerMask);
        }
    }
}
