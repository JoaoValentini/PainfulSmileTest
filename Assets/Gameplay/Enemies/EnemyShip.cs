using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Ships;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies
{
    public class EnemyShip : Ship
    {
        protected NavMeshAgent _navMeshAgent;
        protected virtual EnemyData enemyData {get;}
        public EnemyData EnemyData => enemyData;

        protected delegate void UpdateDelegate();
        protected UpdateDelegate _updateDelegate;

        protected NavMeshPath _path;
        protected bool _isFollowingPath;
        protected Vector3 _destination;
        protected float _pathUpdateRate = 0.25f;
        protected float _nextPathUpdateTime;
        protected float _distanceDamping = .25f;
        protected float _stopMovingThreshold = 0.15f;

        void Awake()
        {
            _path = new NavMeshPath();
            InitializeEnemy();
        }

        protected void Start()
        {
            GameSession.Instance.onGameEnded += OnGameEnded;
        }

        void OnDestroy()
        {
            GameSession.Instance.onGameEnded -= OnGameEnded;
        }

        protected void Update()
        {
            _updateDelegate?.Invoke();
        }

        void InitializeEnemy()
        {
            if(enemyData == null)
            {
                Debug.LogWarning("Enemy Data is null!, destroying enemy");
                Destroy(gameObject);
                return;
            }

            InitializeShip(enemyData.MaxHealth);
            ShipGraphics.SetShipStatesList(enemyData.ShipStates, true);
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.speed = enemyData.Speed;
        }

        protected virtual bool SetDestination(Vector3 destination)
        {
            if(_nextPathUpdateTime > Time.time || _isDestroyed)
                return false;
            if(!_navMeshAgent.CalculatePath(destination, _path))
                return false;
            
            _nextPathUpdateTime = Time.time + _pathUpdateRate;
            _destination = destination;
            _isFollowingPath = true;
                return true;
        }

        protected virtual void MoveToDestination()
        {
            if(!_isFollowingPath)
                return;

            float distance = Vector3.Distance(Position, _destination);
            if(distance <= _stopMovingThreshold)
            {
                OnReachDestination();
                return;
            }

            if(_path.corners.Length > 1)
                LookTowards(_path.corners[1]);
            float speed = enemyData.Speed * Time.deltaTime * Mathf.Clamp01(distance/_distanceDamping);
            _navMeshAgent.Move(GraphicsTransform.up * speed);
        }

        protected virtual void OnReachDestination()
        {
            _isFollowingPath = false;
        }


        public void LookAt(Vector3 targetPos)
        {
            Vector3 dir = targetPos - ShipGraphics.transform.position;

            float angle = Vector3.Angle(dir, Vector3.forward);
            float dirDot = Vector3.Dot(Vector3.left, dir);
            float angleSign = Mathf.Sign(dirDot);

            GraphicsTransform.localRotation = Quaternion.Euler(0,0,angle * angleSign);
        }

        protected void LookTowards(Vector3 destination)
        {
            Vector3 direction = destination - GraphicsTransform.position;
            float angleNeeded = Vector3.Angle(direction, GraphicsTransform.up);
            float dirDot = Vector3.Dot(-GraphicsTransform.right, direction);
            float angleSign = Mathf.Sign(dirDot);
            
            angleNeeded *= angleSign;

            float baseAngleRotation = angleNeeded * Time.deltaTime * enemyData.AngularSpeed / 10;
            GraphicsTransform.localEulerAngles += new Vector3(0,0,baseAngleRotation);
        }

        public override void DestroyShip()
        {
            base.DestroyShip();
            _navMeshAgent.enabled = false;
            GameSession.Instance.onEnemyDefeated?.Invoke(this);
            _isFollowingPath = false;

        }

        protected virtual void OnGameEnded()
        {
            ShipCanvas.HideHealthBar();
        }
    }
}
