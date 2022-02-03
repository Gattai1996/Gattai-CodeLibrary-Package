using UnityEngine;

namespace Gattai.Runtime.Singletons
{
    /// <summary>
    /// A class with a global and static instance, that is not destroyed when switching scenes.
    /// </summary>
    /// <typeparam name="T">The type of the class instance must be the
    /// same type as the class that is inheriting that class.</typeparam>
    /// <code>DerivedClass.Instance.DoSomeStuff();</code>
    public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            if (Instance != null)
            {
                UnityEngine.Debug.Log("Destroying an instance of SingletonPersistent because another instance was found. " +
                                      $"The instances are {gameObject.name} and {Instance.gameObject.name}.");
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }
    }
}