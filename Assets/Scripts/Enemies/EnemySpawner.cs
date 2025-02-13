using System;
using System.Collections;
using System.Collections.Generic;
using Gameflow;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _prefabEnemy;
        [SerializeField] private LevelList _levelList;
        [SerializeField] private EndGame _endGame;

        private GameObjectPool<Enemy> _enemyPool;

        private int _currentLevel = 0;

        private bool _started = false;
        private bool _finishedSpawns = false;
        private List<Enemy> _aliveEnemies = new List<Enemy>();

        private void Awake()
        {
            _enemyPool = new GameObjectPool<Enemy>(_prefabEnemy);
        }

        public void StartSpawns()
        {
            if (_started) return;
            StartCoroutine(TimedSpawns());
            _started = true;
        }

        private IEnumerator TimedSpawns()
        {
            for (int i = 0; i < _levelList.Levels[_currentLevel].Enemies.Length; i++)
            {
                var enemy = _levelList.Levels[0].Enemies[i];
                SpawnEnemy(Random.Range(enemy.size.x, enemy.size.y + 1), Random.Range(enemy.life.x, enemy.life.y + 1));

                yield return new WaitForSeconds(Random.Range(enemy.timeToNext.x, enemy.timeToNext.y));
            }

            _finishedSpawns = true;
        }

        private void SpawnEnemy(int size, int life)
        {
            var enemy = _enemyPool.Get();
            enemy.transform.parent = transform;

            bool randomIsLeft = Random.Range(0, 2) == 0;

            enemy.SetEnemy(size, life, randomIsLeft, this);

            //To position the enemy
            var offsetX = 0f;
            if (enemy.TryGetComponent(out SpriteRenderer renderer))
            {
                offsetX = renderer.bounds.extents.x;
                offsetX = randomIsLeft ? -offsetX : offsetX;
            }

            var posX = CameraLimit.GetCameraXLimit(randomIsLeft) + offsetX;
            enemy.transform.position = new Vector3(posX, enemy.transform.position.y, 1f);

            _aliveEnemies.Add(enemy);
            enemy.OnDeath += () =>
            {
                _enemyPool.Release(enemy);
                if (_aliveEnemies.Contains(enemy)) _aliveEnemies.Remove(enemy);
                CheckForWin();
            };
        }

        public void SpawnEnemyPositioned(int size, int life, Vector3 position, bool goingRight)
        {
            var enemy = _enemyPool.Get();
            enemy.transform.parent = transform;

            enemy.SetEnemy(size, life, goingRight, this, false);

            enemy.transform.position = position;

            _aliveEnemies.Add(enemy);
            enemy.OnDeath += () =>
            {
                _enemyPool.Release(enemy);
                if (_aliveEnemies.Contains(enemy)) _aliveEnemies.Remove(enemy);
                CheckForWin();
            };
        }

        private void CheckForWin()
        {
            if(!_finishedSpawns) return;
            
            if(_aliveEnemies.Count > 0) return;
            
            _endGame.Win();
        }
    }
}