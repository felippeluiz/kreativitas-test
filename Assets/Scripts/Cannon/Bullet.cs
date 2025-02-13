using System;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cannon
{
    public class Bullet : MonoBehaviour
    {
        public event Action OnDeath;
    
        [SerializeField] private float _ylimitOnTop=10f;
        [SerializeField]private float _speed=1f;
    
        void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * _speed,
                transform.position.z);
        
            if(transform.position.y > _ylimitOnTop)
                Die();
        }

        private void Die()
        {
            OnDeath?.Invoke();
            OnDeath = null;
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.gameObject.CompareTag("Enemy")) return;
        
            if(!other.gameObject.TryGetComponent<Enemy>(out var enemy)) return;
        
            enemy.TakeDamage(1);
        
            Die();
        }
    }
}
