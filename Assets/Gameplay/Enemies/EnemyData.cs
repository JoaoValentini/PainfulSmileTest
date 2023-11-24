using System.Collections;
using System.Collections.Generic;
using Gameplay.Ships;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyData : ScriptableObject
    {
        [Header("Common Settings")]
        [SerializeField] float _maxHealth;
        [SerializeField] int _damage;
        [SerializeField] float _speed;
        [SerializeField] float _angularSpeed;
        [SerializeField] List<ShipState> _shipStates;
        
        public float Speed => _speed;
        public float AngularSpeed => _angularSpeed;
        public float MaxHealth => _maxHealth;
        public int Damage => _damage;
        public List<ShipState> ShipStates => _shipStates;

    }
}
