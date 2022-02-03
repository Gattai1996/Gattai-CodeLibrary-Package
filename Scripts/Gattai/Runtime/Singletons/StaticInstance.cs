using UnityEngine;

namespace Gattai.Runtime.Singletons
{
    /// <summary>
    /// A class with a global and static instance.
    /// </summary>
    /// <typeparam name="T">The type of the class instance must be the
    /// same type as the class that is inheriting that class.</typeparam>
    /// <code>DerivedClass.Instance.DoSomeStuff();</code>
    public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        
        protected virtual void Awake() => Instance = this as T;

        protected virtual void OnApplicationQuit()
        {
            Instance = null;
            Destroy(gameObject);
        }
    }
}
