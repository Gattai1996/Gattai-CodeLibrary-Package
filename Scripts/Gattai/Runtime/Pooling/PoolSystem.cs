using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gattai.Runtime.Pooling
{
    /// <summary>
    /// A class representing a Pool of Objects. Use this instead of Instantiate() and Destroy()
    /// methods to improve the performance.
    /// </summary>
    public class PoolSystem : MonoBehaviour
    {
        private static Dictionary<PooledMonoBehaviour, PoolSystem> _poolSystems = 
            new Dictionary<PooledMonoBehaviour, PoolSystem>();

        private Queue<PooledMonoBehaviour> _pooledObjects = new Queue<PooledMonoBehaviour>();

        private List<PooledMonoBehaviour> _disabledPooledObjects = new List<PooledMonoBehaviour>();
    
        private PooledMonoBehaviour _pooledPrefab;

        private int _lastPooledObjectIndex;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            ResetDisabledPooledObjects();
        }

        private void ResetDisabledPooledObjects()
        {
            if (_disabledPooledObjects == null) return;
            
            if (_disabledPooledObjects.Count == 0) return;

            foreach (var disabledObject in _disabledPooledObjects.Where(x =>
                {
                    if (x == null)
                    {
                        return false;
                    }
                    
                    if (x.gameObject == null)
                    {
                        return false;
                    }
                    
                    var o = x.gameObject;
                    return o != null && !o.activeInHierarchy;
                }))
            {
                disabledObject.transform.SetParent(transform);
            }
            
            _disabledPooledObjects.Clear();
        }
        
        /// <summary>
        /// Get a Pool System by passing a Pooled Prefab.
        /// </summary>
        /// <param name="pooledPrefab">The Pooled Prefab that you want to get the Pool System.</param>
        /// <returns>Pool System.</returns>
        public static PoolSystem GetPoolSystem(PooledMonoBehaviour pooledPrefab)
        {
            if (_poolSystems.ContainsKey(pooledPrefab))
            {
                return _poolSystems[pooledPrefab];
            }

            var poolSystem = new GameObject("Pool-" + pooledPrefab.name).AddComponent<PoolSystem>();
            
            poolSystem._pooledPrefab = pooledPrefab;

            _poolSystems.Add(pooledPrefab, poolSystem);

            return poolSystem;
        }

        /// <summary>
        /// Get a Prefab from the Pool System.
        /// </summary>
        /// <typeparam name="T">The type of the Prefab.</typeparam>
        /// <returns>The instance of the Prefab.</returns>
        public T Get<T>() where T : PooledMonoBehaviour
        {
            if (_pooledObjects.Count == 0)
            {
                GrowPool();
            }

            var pooledObject = _pooledObjects.Dequeue();

            return pooledObject as T;
        }

        /// <summary>
        /// Instantiate a Pooled Prefab and add it to the Pool System.
        /// </summary>
        public void GrowPool()
        {
            for (var i = 0; i < _pooledPrefab.InitialPoolSize; i++)
            {
                var pooledObject = Instantiate(_pooledPrefab, GetPoolSystem(_pooledPrefab).transform);

                pooledObject.gameObject.name += $" ({_lastPooledObjectIndex})";

                _lastPooledObjectIndex++;

                pooledObject.OnDestroyAction += () => SetObjectAvailable(pooledObject);
                
                pooledObject.gameObject.SetActive(false);
            }
        }

        private void SetObjectAvailable(PooledMonoBehaviour pooledObject)
        {
            _disabledPooledObjects.Add(pooledObject);
            _pooledObjects.Enqueue(pooledObject);
        }

        public void RemoveObject<T>(T pooledObject) where T : PooledMonoBehaviour
        {
            if (_disabledPooledObjects.Contains(pooledObject))
            {
                _disabledPooledObjects.Remove(pooledObject);
            }

            DestroyImmediate(pooledObject);
        }
    }
}