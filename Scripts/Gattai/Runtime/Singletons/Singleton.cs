using UnityEngine;

namespace Gattai.Runtime.Singletons
{
    /// <summary>
    /// A class with a single, global and static instance.
    /// </summary>
    /// <typeparam name="T">The type of the class instance must be the
    /// same type as the class that is inheriting that class.</typeparam>
    /// <code>DerivedClass.Instance.DoSomeStuff();</code>
    public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            if (Instance != null)
            {
                UnityEngine.Debug.LogWarning($"Destroying an instance of Singleton because another instance was found. " +
                                             $"The instances are {gameObject.name} and {Instance.gameObject.name}. " +
                                             $"Destroying {gameObject.name}.");
                
                Destroy(gameObject);
            }
            
            base.Awake();
        }
    }
}
