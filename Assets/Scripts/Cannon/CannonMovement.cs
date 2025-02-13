using System;
using UnityEngine;
using Util;

namespace Cannon
{
    /// <summary>
    /// Handles movement for the cannon, it will follow the finger position, only on the X axis,
    /// with a limit on both sides of the screen.
    /// </summary>
    public class CannonMovement : MonoBehaviour
    {
        [SerializeField] private Transform _cannonTransform;

        [SerializeField] private float _timeToCrossScreen = 0.1f;

        private Vector3 _initialCannonPosition;

        private float _minPositionX = -2.5f;
        private float _maxPositionX = 2.5f;
        private float _totalXtoMove;
        private float _totalAmountMoved;
        private float _totalScreenXSpace;
        private void Start()
        {
            _minPositionX = CameraLimit.GetCameraXLimit(true);
            _maxPositionX = CameraLimit.GetCameraXLimit(false);
            
            _initialCannonPosition = _cannonTransform.position;
            _totalScreenXSpace = _maxPositionX - _minPositionX;
        }

        public void OnDragStarted(Vector3 worldPosition)
        {
            CalculateAmountToMove(worldPosition);
        }
        public void OnDragUpdated(Vector3 worldPosition)
        {
            CalculateAmountToMove(worldPosition);
        }

        private void Update()
        {
            MoveCannon();
        }

        private void CalculateAmountToMove(Vector3 worldPosition)
        {
            float deltaX = worldPosition.x - _cannonTransform.position.x;

            _totalXtoMove = deltaX;
            _totalAmountMoved = 0;
        }

        private void MoveCannon()
        {
            if(_totalXtoMove==0) return;
            
            if(Mathf.Abs(_totalAmountMoved)>=Mathf.Abs(_totalXtoMove))
            {
                _totalXtoMove = 0;
                _totalAmountMoved = 0;
                return;
            }
            
            _initialCannonPosition = _cannonTransform.position;
            
            var min = -(Math.Abs(_totalXtoMove));
            
            float amountMove = Mathf.Clamp(_totalXtoMove * (Time.deltaTime/ ((Math.Abs(_totalXtoMove)/_totalScreenXSpace)*_timeToCrossScreen)),min,Math.Abs(_totalXtoMove));
            
            _totalAmountMoved += amountMove;

            float newX = Mathf.Clamp
            (
                _initialCannonPosition.x + amountMove,
                _minPositionX,
                _maxPositionX
            );

            if (newX < _minPositionX || newX > _maxPositionX) _totalAmountMoved = 0;
            
            _cannonTransform.position = new Vector3(newX, _initialCannonPosition.y, _initialCannonPosition.z);
        }
    }
}