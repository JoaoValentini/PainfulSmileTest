using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        PlayerShip _playerShip;
        Transform _graphicsTransform;
        
        // Movement
        [SerializeField] float _movementSpeed = 5;
        [SerializeField] float _movementDampingTime = 1.5f;
        float _desiredMovementInput;
        float _currentMovementInput;
        float _currentMovementDamping;
        
        //Rotation
        [SerializeField] float _rotationSpeed = 200;
        [SerializeField] float _rotationDampingTime = 1.5f;
        float _desiredRotationInput;
        float _currentRotationInput;
        float _currentRotationDamping;

        internal void Initialize(PlayerShip playerShip)
        {
            _playerShip = playerShip;

            _graphicsTransform = _playerShip.ShipGraphics.transform;
        }

        void FixedUpdate()
        {
            CalculateMovement();
            CalculateRotation();
        }

        void OnMove(InputValue inputValue)
        {
            _desiredMovementInput = inputValue.Get<float>();
            _currentMovementDamping = 0;
        }
        void CalculateMovement()
        {
            if(_currentMovementInput != _desiredMovementInput)
            {
                _currentMovementDamping += Time.fixedDeltaTime / _movementDampingTime;
                _currentMovementInput = Mathf.Lerp(_currentMovementInput, _desiredMovementInput, _currentMovementDamping);
            }
            _playerShip.ShipRigidbody.velocity = _currentMovementInput * _movementSpeed * _graphicsTransform.up;
        }

        void OnRotate(InputValue inputValue)
        {
            _desiredRotationInput = inputValue.Get<float>();
            _currentRotationDamping = 0;
        }
        void CalculateRotation()
        {
            if(_currentRotationInput != _desiredRotationInput)
            {
                _currentRotationDamping += Time.fixedDeltaTime / _rotationDampingTime;
                _currentRotationInput = Mathf.Lerp(_currentRotationInput, _desiredRotationInput, _currentRotationDamping);
            }
            _graphicsTransform.Rotate(0, 0,_currentRotationInput * _rotationSpeed * Time.deltaTime);
        }
    }
}
