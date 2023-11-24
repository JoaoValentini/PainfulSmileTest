using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies
{
    public class Chaser : EnemyShip
    {
        [SerializeField] ChaserData _chaserData;
        protected override EnemyData enemyData { get => _chaserData;}

        enum ChaserState {Idle, Chasing}
        ChaserState _currentState = ChaserState.Idle;

        [SerializeField] ParticleSystem _explosionParticlesPrefab;

        new void Start()
        {
            base.Start();
            ChangeState(ChaserState.Chasing);
        }

        void ChasingUpdate()
        {
            SetDestination(GameSession.Instance.Player.Position);
            MoveToDestination();
        }

        protected override void MoveToDestination()
        {
            float distance = Vector3.Distance(Position, _destination);

            if(_path.corners.Length > 1)
                LookTowards(_path.corners[1]);
            float speed = enemyData.Speed * Time.deltaTime * Mathf.Clamp01(distance/_distanceDamping);
            _navMeshAgent.Move(GraphicsTransform.up * speed);
        }


        void Explode()
        {
            ParticleSystem explosion = Instantiate(_explosionParticlesPrefab, Position, Quaternion.Euler(-90,0,0));
            float size = _chaserData.ExplosionRadius;
            explosion.transform.localScale = new Vector3(size, size, size);
            ShipGraphics.InstantiateDebris();
            
            Destroy(gameObject);
        }
        
        void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.TryGetComponent<Player.PlayerShip>(out Player.PlayerShip player))
            {
                player.OnDamaged(_chaserData.Damage);
                Explode();
            }
        }

        void ChangeState(ChaserState newState)
        {
            if(newState == _currentState)
                return;

            _currentState = newState;
            switch (_currentState)
            {
                case ChaserState.Chasing:
                    _updateDelegate = ChasingUpdate;
                return;
                default: 
                    _updateDelegate = null;
                return;
            }
        }

        protected override void OnGameEnded()
        {
            base.OnGameEnded();
            ChangeState(ChaserState.Idle);
        }

        public override void DestroyShip()
        {
            base.DestroyShip();
        }

     
    }
}
