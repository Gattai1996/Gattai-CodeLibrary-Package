using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gattai.Runtime.Pooling
{
    /// <summary>
    /// Used to pre-grow a Pool System.
    /// </summary>
    public class PoolPreparer : MonoBehaviour
    {
        [SerializeField]
        private PooledMonoBehaviour[] prefabs;

        [SerializeField]
        private int initialPoolSize = 3;

        private void Awake()
        {
            foreach (var prefab in prefabs)
            {
                if (prefab == null)
                {
                    UnityEngine.Debug.LogError("Null prefab on array of prefabs on " +
                                               $"{nameof(PoolPreparer)} '{gameObject.name}'.");
                    return;
                }
                
                prefab.SetInitialPoolSize(initialPoolSize);
                PoolSystem.GetPoolSystem(prefab).GrowPool();
            }
        }

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            var prefabsToRemove = new List<GameObject>();

            foreach (var prefab in prefabs.Where(x => x != null))
            {
                if (PrefabUtility.GetPrefabAssetType(prefab) != PrefabAssetType.NotAPrefab) continue;
                
                UnityEngine.Debug.LogError($"{prefab.gameObject.name} is not a prefab. " +
                                           "It has been removed from the prefabs array on " +
                                           $"{nameof(PoolPreparer)} '{gameObject.name}'.");
                
                prefabsToRemove.Add(prefab.gameObject);
            }

            prefabs = prefabs
                .Where(x => x != null && !prefabsToRemove.Contains(x.gameObject))
                .ToArray();
        }
        
#endif
        
    }
}
