using UnityEngine;
using UnityEngine.Pool;

namespace Util
{
    /// <summary>
    /// A generic object pool for GameObjects with a specified Component type.
    /// </summary>
    /// <typeparam name="T">The type of Component that will be pooled. Must inherit from Component.</typeparam>
    public class GameObjectPool<T> where T : Component
    {
        private readonly T _prefab;
        private ObjectPool<T> _pool;

        private int _counter = 0;

        /// <summary>
        /// Initializes a new instance of the GameObjectPool class.
        /// </summary>
        /// <param name="prefab">The prefab used to instantiate objects in the pool.</param>
        public GameObjectPool(T prefab)
        {
            _prefab = prefab;
            _pool = new ObjectPool<T>(Create, OnGet, OnRelease, OnDestroy, defaultCapacity: 150, maxSize: 500,
                collectionCheck: false);
        }

        /// <summary>
        /// Creates a new instance of the prefab for the pool.
        /// </summary>
        /// <returns>A new instance of the prefab.</returns>
        private T Create()
        {
            _counter++;
            var obj = Object.Instantiate(_prefab);
            obj.gameObject.name = $"{_prefab.name} {_counter}";
            return obj;
        }

        /// <summary>
        /// Called when an object is retrieved from the pool.
        /// Activates the GameObject.
        /// </summary>
        /// <param name="component">The component being retrieved.</param>
        private void OnGet(T component)
        {
            component.gameObject.SetActive(true);
        }

        /// <summary>
        /// Called when an object is returned to the pool.
        /// Deactivates the GameObject.
        /// </summary>
        /// <param name="component">The component being returned.</param>
        private void OnRelease(T component)
        {
            component.gameObject.SetActive(false);
        }

        /// <summary>
        /// Called when an object is destroyed by the pool.
        /// Destroys the associated GameObject.
        /// </summary>
        /// <param name="component">The component to destroy.</param>
        private void OnDestroy(T component)
        {
            Object.Destroy(component.gameObject);
        }

        /// <summary>
        /// Retrieves an instance from the pool.
        /// </summary>
        /// <returns>An instance of T from the pool.</returns>
        public T Get()
        {
            return _pool.Get();
        }

        /// <summary>
        /// Returns an instance back to the pool.
        /// </summary>
        /// <param name="component">The component to release back into the pool.</param>
        public void Release(T component)
        {
            _pool.Release(component);
        }
    }
}