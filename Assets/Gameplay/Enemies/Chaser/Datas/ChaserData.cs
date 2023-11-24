using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Enemies
{
    [CreateAssetMenu(menuName = "Enemy/Chaser Data", fileName = "ChaserData")]
    public class ChaserData : EnemyData
    {
        [Header("Chaser Settings")]
        [SerializeField] float _explosionRadius;
        [SerializeField] float _distanceFromPlayerToExplode;
        [SerializeField] float _explosionDelay;
        
        public float ExplosionRadius => _explosionRadius;
        public float DistanceFromPlayerToExplode => _distanceFromPlayerToExplode;
        public float ExplosionDelay => _explosionDelay;
    }
}
