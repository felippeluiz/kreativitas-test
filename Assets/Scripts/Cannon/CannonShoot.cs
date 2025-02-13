using System.Collections;
using UnityEngine;
using Util;

namespace Cannon
{
    public class CannonShoot : MonoBehaviour
    {        
        [SerializeField] private CannonParameters _cannonParameters;

        [Header("Bullet spawn")]
        [SerializeField] private Bullet _prefabBullet;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _bulletsParent;

        [Header("Cannon Audio")]
        [SerializeField] private AudioSource _cannonSource;

        private GameObjectPool<Bullet> _bulletsPool;
        private bool _canShoot = true;

        private void Awake()
        {
            _bulletsPool = new GameObjectPool<Bullet>(_prefabBullet);
        }

        /// <summary>
        /// Tries to shoot the cannon if it is not on cooldown.
        /// </summary>
        public void TryingToShoot()
        {
         if(!_canShoot) return;

         _canShoot = false;
         Shoot();
        }

        private void Shoot()
        {
            //Randomize the pitch and play the audioSource of the cannon
            _cannonSource.pitch = Random.Range(0.8f, 1.2f);
            _cannonSource.Play();

            var bullet = _bulletsPool.Get();
            bullet.transform.parent = _bulletsParent;
            bullet.transform.position = _spawnPoint.position;
            bullet.OnDeath += () => _bulletsPool.Release(bullet);
            
            StartCoroutine(DelayToShot());
        }

        private IEnumerator DelayToShot()
        {
            yield return new WaitForSeconds(1 / _cannonParameters.ShotsPerSecond);
            _canShoot = true;
        }
    }
}