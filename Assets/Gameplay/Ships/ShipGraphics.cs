using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Ships
{
    public class ShipGraphics : MonoBehaviour
    {
        Ship _ship;
        Transform _transform;
        public Transform Transform => _transform;
        [SerializeField] SpriteRenderer _hull;
        [SerializeField] SpriteRenderer _mainSail;
        [SerializeField] SpriteRenderer _smallSail;
        [SerializeField] SpriteRenderer _mast;
        [SerializeField] SpriteRenderer _mastPole;

        [Space]
        [SerializeField] ParticleSystem _destructionParticlesPrefab;
        [SerializeField] GameObject _debrisPrefab;
        
        [Space]
        [SerializeField] List<ShipState> _shipStates = new List<ShipState>();
        int _currentDeteriorationState = -1;

        void Awake()
        {
            _transform = transform;
        }
        internal void Initialize(Ship ship)
        {
            _ship = ship;
            _ship.onDamaged += OnDamaged;
            _ship.onDestroyed += InstantiateDestructionParticles;
        }

        public void SetShipStatesList(List<ShipState> shipStates, bool updateGraphics)
        {
            _shipStates = shipStates;
            if(updateGraphics && _shipStates.Count > 0 && _ship)
            {
                CheckForDeterioration(_ship.CurrentHealth, _ship.MaxHealth);
            }
        }

        public void UpdateGraphics(ShipSkin shipSkin)
        {
            if(!shipSkin)
            {
                Debug.Log("Ship Skin is null");
                return;
            }
            _hull.sprite = shipSkin.Hull;
            _mainSail.sprite = shipSkin.MainSail;
            _smallSail.sprite = shipSkin.SmallSail;
            _mast.sprite = shipSkin.Mast;
            _mastPole.sprite = shipSkin.MastPole;
        }

        void OnDamaged(float currentHealth, float maxHealth)
        {
            CheckForDeterioration(currentHealth, maxHealth);
        }

        void CheckForDeterioration(float currentHealth, float maxHealth)
        {
            int newDeteriorationState = _shipStates.GetDeteriotaionStateIndex(currentHealth, maxHealth);
            if(newDeteriorationState == _currentDeteriorationState || newDeteriorationState == -1)
                return;

            _currentDeteriorationState = newDeteriorationState;
            UpdateGraphics(_shipStates[_currentDeteriorationState].ShipSkin);
        }

        void InstantiateDestructionParticles()
        {
            if(_destructionParticlesPrefab)
                Instantiate(_destructionParticlesPrefab, _ship.Position, Quaternion.Euler(90,0,Transform.localEulerAngles.z));
            
            InstantiateDebris();
        }

        public void InstantiateDebris()
        {
            if(_debrisPrefab)
            {
                float angle = Random.Range(0,360);
                Instantiate(_debrisPrefab, _ship.Position, Quaternion.Euler(0,angle,0));
            }
        }
    }
}
