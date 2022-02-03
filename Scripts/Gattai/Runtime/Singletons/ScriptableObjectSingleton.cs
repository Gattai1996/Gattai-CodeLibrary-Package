using System.Linq;
using UnityEngine;

namespace Gattai.Runtime.Singletons
{
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T> 
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance) return _instance;
                
                var assets = Resources.LoadAll<T>("");
                
                if (assets.Length > 1)
                {
                    UnityEngine.Debug.LogWarning($"Multiple instances of {typeof(T)} has found on resources. " +
                                                 $"Using the first one ({assets[0].name}).");
                }
                    
                _instance = assets.FirstOrDefault();
                
                if (_instance != null)
                {
                    _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                }
                else
                {
                    UnityEngine.Debug.LogError($"Could not find any {typeof(T)} on resources.");
                    return null;
                }

                return _instance;
            }
        }

        protected virtual void OnDestroy() => _instance = null;
    }
}
