using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] ParticleSystem _hitParticlePrefab;
        float _damage;

        public void Shoot(Vector3 force, float damage)
        {
            _damage = damage;
            _rigidbody.AddForce(force, ForceMode.Impulse);
            Invoke("Destroy", 3);
        }
        public void Shoot(Vector3 force, float damage, LayerMask layersToHit)
        {
            _rigidbody.includeLayers = layersToHit;
            Shoot(force, damage);
        }

        void OnCollisionEnter(Collision coll)
        {
            if(coll.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.OnDamaged(_damage);
            }

            if(_hitParticlePrefab)
                Instantiate(_hitParticlePrefab, coll.GetContact(0).point, Quaternion.identity);
            Destroy(gameObject);
        }

        void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
