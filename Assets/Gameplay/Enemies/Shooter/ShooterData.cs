using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Enemies
{
    [CreateAssetMenu(menuName = "Enemy/Shooter Data", fileName = "ShooterData")]
    public class ShooterData : EnemyData
    {
        [Header("Shooter Settings")]
        [SerializeField] float _maxShootingDistance;
        [SerializeField] float _distanceToMoveFromPlayer;
        [SerializeField] float _distanceToMoveFromPlayerThreshold;
        [SerializeField][Range(30, 180)] float _maxRepositioningAngle;
        [SerializeField] float _projectileSpeed;
        [SerializeField] float _firstShotDelay;
        [SerializeField][Range(1,5)] int _shotsPerCharge;
        [SerializeField] float _delayBetweenShots;
        [SerializeField] float _rechargeTimeSeconds;

        public float MaxShootingDistance => _maxShootingDistance;
        public float DistanceToMoveFromPlayer => _distanceToMoveFromPlayer;
        public float DistanceToMoveFromPlayerThreshold => _distanceToMoveFromPlayerThreshold;
        public float MaxRepositioningAngle => _maxRepositioningAngle;
        public float ProjectileSpeed => _projectileSpeed;
        public float FirstShotDelay => _firstShotDelay;
        public float DelayBetweenShots => _delayBetweenShots;
        public int ShotsPerCharge => _shotsPerCharge;
        public float RechargeTimeSeconds => _rechargeTimeSeconds;
    }
}
