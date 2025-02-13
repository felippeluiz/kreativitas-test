using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _prefabEnemy;

        private GameObjectPool<Enemy> _enemyPool;

        private void Awake()
        {
            _enemyPool = new GameObjectPool<Enemy>(_prefabEnemy);
        }
        
        void Update()
        {
            if (Input.GetButtonDown("Jump")) SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            var enemy = _enemyPool.Get();
            enemy.transform.parent = transform;
            
            bool randomIsLeft = Random.Range(0, 2) == 0; 
            
            enemy.SetEnemy(Random.Range(0, 4), Random.Range(1, 30), randomIsLeft);
            
            //To position the enemy
            var offsetX = 0f;
            if (enemy.TryGetComponent(out SpriteRenderer renderer))
            {
                offsetX = renderer.bounds.extents.x;
                offsetX = randomIsLeft ? -offsetX : offsetX;
            }

            var posX = CameraLimit.GetCameraXLimit(randomIsLeft) + offsetX;
            enemy.transform.position = new Vector3(posX, enemy.transform.position.y, 1f);

            enemy.OnDeath += () => _enemyPool.Release(enemy);
        }
    }
}