using UnityEngine;
using UnityEngine.Events;

namespace Gattai.Runtime.Events
{
    public class EventListener : MonoBehaviour
    {
        public EventEmitter eventEmitter;
        public UnityEvent unityEvent;

        private void OnEnable()
        {
            eventEmitter.Register(this);
        }

        private void OnDisable()
        {
            eventEmitter.Unregister(this);
        }

        public void TriggerEvent()
        {
            unityEvent.Invoke();
        }
    }
}