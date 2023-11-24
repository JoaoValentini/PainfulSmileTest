using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Ships
{
    public class Ship : MonoBehaviour, IDamageable
    {
        [SerializeField] ShipGraphics _shipGraphics;
        [SerializeField] ShipCanvas _shipCanvas;
        [SerializeField] Rigidbody _shipRigidbody;
        [SerializeField] Collider _shipCollider;

        public ShipGraphics ShipGraphics => _shipGraphics;
        public ShipCanvas ShipCanvas => _shipCanvas;
        public Rigidbody ShipRigidbody => _shipRigidbody;
        public Collider ShipCollider => _shipCollider;
        public Transform GraphicsTransform => _shipGraphics.Transform;
        
        Transform _shipTransform;
        public Transform ShipTransform => _shipTransform;
        public Vector3 Position => _shipTransform.position;

        public event Action<float, float> onDamaged;
        public event Action onDestroyed;

        protected float currentHealth;
        protected float maxHealth;
        protected bool _isDestroyed = false;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;

        protected virtual void InitializeShip(float maxHealth)
        {
            _shipTransform = transform;
            this.maxHealth = maxHealth;
            currentHealth = maxHealth;

            _shipCanvas.Initialize(this);
            _shipGraphics.Initialize(this);
        }

        public virtual void OnDamaged(float damage)
        {
            if(_isDestroyed)
                return;

            currentHealth -= damage;
            onDamaged?.Invoke(currentHealth, maxHealth);

            if(currentHealth <= 0)
            {
                DestroyShip();
            }
        }

        public virtual void DestroyShip()
        {
            onDestroyed?.Invoke();
            _isDestroyed = true;
            Destroy(gameObject);
        }

        protected void DisableShipCollider()
        {
            if(_shipCollider)
                _shipCollider.enabled = false;
        }
    }
}
