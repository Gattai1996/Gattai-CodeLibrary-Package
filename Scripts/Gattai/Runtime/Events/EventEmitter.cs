using System.Collections.Generic;
using UnityEngine;

namespace Gattai.Runtime.Events
{
    [CreateAssetMenu(fileName = "New Event Emmiter", menuName = "Event Emmiter", order = 0)]
    public class EventEmitter : ScriptableObject
    {
        private List<EventListener> _eventListeners = new List<EventListener>();

        public void Register(EventListener eventListener)
        {
            _eventListeners.Add(eventListener);
        }

        public void Unregister(EventListener eventListener)
        {
            _eventListeners.Remove(eventListener);
        }

        public void TriggerEvent()
        {
            foreach (var eventListener in _eventListeners)
            {
                eventListener.TriggerEvent();
            }
        }
    }
}