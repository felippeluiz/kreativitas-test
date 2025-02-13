using System;
using TMPro;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public event Action OnDeath;
    
    [SerializeField] private EnemyParameters _parameters;
    [SerializeField] private TextMeshPro _txtLife;
    [SerializeField] private SpriteRenderer _renderer;
    
    private int _size;
    private int _lifeValue;
    private bool _enteringScreen = true;
    private bool _goingRight = false;
    private bool _goingDown = true;
    private float _bounceTime=1f;
    private float _totalBounceTime=0f;
    private float _rotateSpeed = 1f;

    public void SetEnemy(int size, int life, bool goingRight, bool isEnteringScreen=true)
    {
        _size = size;
        _lifeValue = life;
        _txtLife.text = _lifeValue.ToString();
        
        var safeSize = size < _parameters.SizesParameters.Length ? size : _parameters.SizesParameters.Length - 1;
        
        var scale = _parameters.SizesParameters[safeSize].scale;
        
        transform.localScale = new Vector3(scale, scale, 1);

        transform.position = new Vector3(transform.position.x,_parameters.SizesParameters[safeSize].jumpYHeight+_renderer.bounds.extents.y,transform.position.z);
        
        UpdateColor();
        
        _goingRight = goingRight;
        _enteringScreen = isEnteringScreen;
        _goingDown = true;
        
        var totalHeight = _parameters.SizesParameters[_size].jumpYHeight - _parameters.FloorY;
        var totalHeightSize1 = _parameters.SizesParameters[0].jumpYHeight - _parameters.FloorY;
        _totalBounceTime = (totalHeight / totalHeightSize1) * _parameters.Size1BounceTime;
        
        _bounceTime=_totalBounceTime/2f;

        _rotateSpeed = Random.Range(-15f, 15f);
    }

    private void Update()
    {
        var moveX = _goingRight ? _parameters.Speed.x : -_parameters.Speed.x;
        
        moveX *= Time.deltaTime;
        var moveVector = Vector3.zero;

        if (_enteringScreen)
        {
            moveX /= 2;
            CheckEnteringScreen();
        }
        else
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

        moveVector += new Vector3(moveX, 0, 0);
        transform.position += moveVector;
        
        CheckChangeXDirection();
        
        transform.Rotate(Vector3.forward, _rotateSpeed*Time.deltaTime);
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
        Color.RGBToHSV(_parameters.HighLifeColor, out var h2, out var s2, out var v2);
        
        float newHue = Mathf.Lerp(h1, h2, normalizedLife);
        
        Color newColor =
            Color.HSVToRGB(newHue, s1, v1);

        _renderer.color = newColor;
    }

    public void TakeDamage(int damage)
    {
        _lifeValue = _lifeValue - damage < 0 ? 0 : _lifeValue - damage;
        _txtLife.text = _lifeValue.ToString();
        
        if (_lifeValue<=0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        OnDeath = null;
    }
}