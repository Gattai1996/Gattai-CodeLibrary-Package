using System;
using UnityEngine;

namespace Gattai.Runtime.Pooling
{
    /// <summary>
    /// A base class that any Pooled Objects need to inherit from to use the Pool System.
    /// </summary>
    public abstract class PooledMonoBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The initial size of the Pool System of this Prefab.
        /// </summary>
        [SerializeField]
        private int initialPoolSize = 3;
        public int InitialPoolSize => initialPoolSize;
        
        /// <summary>
        /// An Action invoked when the Prefab is destroyed (that is,
        /// disabled and returned to available in Pool System).
        /// </summary>
        public event Action OnDestroyAction;

        /// <summary>
        /// Be sure to override this method and call 'base.OnDisable()' if need to use OnDisable on inheritors classes.
        /// </summary>
        protected virtual void OnDisable()
        {
            OnDestroyAction?.Invoke();
        }

        internal void SetInitialPoolSize(int newInitialPoolSize)
        {
            initialPoolSize = newInitialPoolSize;
        }

        /// <summary>
        /// Get a instance of this Prefab from the Pool System.
        /// </summary>
        /// <param name="activeInHierarchy">Set the instance active by default or not.</param>
        /// <typeparam name="T">The type of this Prefab.</typeparam>
        /// <returns>The instance of this Prefab.</returns>
        public T GetFromPool<T>(bool activeInHierarchy = true) where T : PooledMonoBehaviour
        {
            var poolSystem = PoolSystem.GetPoolSystem(this);
            var pooledObject = poolSystem.Get<T>();

            try
            {
                pooledObject.gameObject.SetActive(activeInHierarchy);
            }
            catch (Exception e)
            {
                poolSystem.RemoveObject(pooledObject);
                GetFromPool<T>();
            }

            return pooledObject;
        }

        /// <summary>
        /// Get a instance of this Prefab from the Pool System and set a parent and reset the position and rotation.
        /// </summary>
        /// <param name="parent">The parent of the instance.</param>
        /// <param name="resetTransformPositionAndRotation">Reset the position and rotation or not.</param>
        /// <typeparam name="T">The type of this Prefab.</typeparam>
        /// <returns>The instance of this Prefab.</returns>
        public T GetFromPool<T>(Transform parent, bool resetTransformPositionAndRotation = false) 
            where T : PooledMonoBehaviour
        {
            var pooledObject = GetFromPool<T>();
            var objTransform = pooledObject.transform;
            
            objTransform.SetParent(parent);
            objTransform.position = resetTransformPositionAndRotation ? Vector3.zero : objTransform.position;
            objTransform.rotation = resetTransformPositionAndRotation ? Quaternion.identity : objTransform.rotation;

            return pooledObject;
        }

        /// <summary>
        /// Get a instance of this Prefab from the Pool System and set a position, rotation and an optional parent.
        /// </summary>
        /// <param name="position">The position to spawn the instance.</param>
        /// <param name="rotation">The rotation of the instance.</param>
        /// <param name="parent">An optional parent to the instance.</param>
        /// <typeparam name="T">The type of this Prefab.</typeparam>
        /// <returns>The instance of this Prefab.</returns>
        public T GetFromPool<T>(Vector3 position, Quaternion rotation, Transform parent = null) where T : PooledMonoBehaviour
        {
            var pooledObject = GetFromPool<T>();

            pooledObject.transform.SetPositionAndRotation(position, rotation);
            
            if (parent != null)
            {
                pooledObject.transform.SetParent(parent);
            }

            return pooledObject;
        }
    }
}