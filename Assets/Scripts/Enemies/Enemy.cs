using System;
using TMPro;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        public event Action OnDeath;
    
        [SerializeField] private EnemyParameters _parameters;
        [SerializeField] private TextMeshPro _txtLife;
        [SerializeField] private SpriteRenderer _renderer;
    
        private EnemySpawner _enemySpawner;
        private int _size;
        private int _lifeValue;
        private int _totalLife;
        private float _bounceTime=1f;
        private float _totalBounceTime=0f;
        private float _firstBounceTime=0f;
        private float _firstBounceHeight = 0f;
        private float _rotateSpeed = 1f;
        private bool _enteringScreen = true;
        private bool _goingRight = false;
        private bool _firstBounce = false;

        public void SetEnemy(int size, int life, bool goingRight,EnemySpawner spawner, bool isEnteringScreen=true)
        {
            _size = size;
            _totalLife = life;
            _lifeValue = _totalLife;
            _txtLife.text = _lifeValue.ToString();
        
            var safeSize = size < _parameters.SizesParameters.Length ? size : _parameters.SizesParameters.Length - 1;
        
            var scale = _parameters.SizesParameters[safeSize].scale;
        
            transform.localScale = new Vector3(scale, scale, 1);

            if (isEnteringScreen)
            {
                transform.position = new Vector3(transform.position.x,
                    _parameters.SizesParameters[safeSize].jumpYHeight + _renderer.bounds.extents.y,
                    transform.position.z);
            }
            else
            {
                _firstBounce = false;
                _firstBounceHeight = transform.position.y;
            }

            UpdateColor();
        
            _goingRight = goingRight;
            _enteringScreen = isEnteringScreen;
        
            var totalHeight = _parameters.SizesParameters[_size].jumpYHeight - _parameters.FloorY;
            var totalHeightSize1 = _parameters.SizesParameters[0].jumpYHeight - _parameters.FloorY;
            _totalBounceTime = (totalHeight / totalHeightSize1) * _parameters.Size1BounceTime;
        
            _bounceTime=_totalBounceTime/2f;

            _rotateSpeed = Random.Range(-15f, 15f);
            _enemySpawner = spawner;
        }

        private void Update()
        {
            var moveX = _goingRight ? _parameters.Speed.x : -_parameters.Speed.x;
        
            moveX *= Time.deltaTime;
            var moveVector = Vector3.zero;

            if (_enteringScreen)
            {
                CheckEnteringScreen();
            }
            else
            {
                if (!_firstBounce)
                {
                    DefaultBounce();
                }
                else
                {
                    FirstBounce();
                }
            }

            moveVector += new Vector3(moveX, 0, 0);
            transform.position += moveVector;
        
            CheckChangeXDirection();
        
            transform.Rotate(Vector3.forward, _rotateSpeed*Time.deltaTime);
        }

        private void DefaultBounce()
        {
            var totalHeight = _parameters.SizesParameters[_size].jumpYHeight - _parameters.FloorY;

            if (_bounceTime > _totalBounceTime)
            {
                _bounceTime = 0f; 
            }

            var posY = _parameters.FloorY + 
                       _renderer.bounds.extents.y +
                       (_parameters.BounceCurve.Evaluate(_bounceTime / _totalBounceTime) * totalHeight);
            
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            
            _bounceTime += Time.deltaTime;
        }

        private void FirstBounce()
        {
            var currentHeight = transform.position.y - _parameters.FloorY;
            var totalHeight = _parameters.SizesParameters[_size].jumpYHeight - _parameters.FloorY;
            if (_firstBounceTime == 0f)
            {
                _firstBounceTime = currentHeight / totalHeight * _totalBounceTime;
                _bounceTime = _firstBounceTime / 2f;
            }
            
            var posY = _parameters.FloorY + 
                       _renderer.bounds.extents.y +
                       (_parameters.BounceCurve.Evaluate(_bounceTime / _totalBounceTime) * _firstBounceHeight);
            
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            
            _bounceTime += Time.deltaTime;
            if (_bounceTime >= _firstBounceTime)
            {
                _firstBounce = false;
                _bounceTime = 0f;
            }
        }

        private void CheckEnteringScreen()
        {
            if (_goingRight)
            {
                _enteringScreen = _renderer.bounds.min.x < CameraLimit.GetCameraXLimit(true);
            }
            else
            {
                _enteringScreen = _renderer.bounds.max.x > CameraLimit.GetCameraXLimit(false);
            }
        }

        private void CheckChangeXDirection()
        {
            if (_goingRight)
            {
                if (_renderer.bounds.max.x > CameraLimit.GetCameraXLimit(false)) _goingRight = false;
            }
            else
            {
                if (_renderer.bounds.min.x < CameraLimit.GetCameraXLimit(true)) _goingRight = true;
            }
        }
    
        private void UpdateColor()
        {
            if (!_parameters) return;
        
            float normalizedLife = Mathf.InverseLerp(1, 25, _lifeValue);
        
            Color.RGBToHSV(_parameters.LowLifeColor, out var h1, out var s1, out var v1);
            Color.RGBToHSV(_parameters.HighLifeColor, out var h2, out _, out _);
        
            float newHue = Mathf.Lerp(h1, h2, normalizedLife);
        
            Color newColor =
                Color.HSVToRGB(newHue, s1, v1);

            _renderer.color = newColor;
        }

        public void TakeDamage(int damage)
        {
            _lifeValue = _lifeValue - damage < 0 ? 0 : _lifeValue - damage;
            _txtLife.text = _lifeValue.ToString();
            UpdateColor();
            if (_lifeValue<=0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (_size > 0)
            {
                _enemySpawner.SpawnEnemyPositioned(_size-1,_totalLife,transform.position, true);                
                _enemySpawner.SpawnEnemyPositioned(_size-1,_totalLife,transform.position, false);                
            }
            OnDeath?.Invoke();
            OnDeath = null;
        }
    }
}