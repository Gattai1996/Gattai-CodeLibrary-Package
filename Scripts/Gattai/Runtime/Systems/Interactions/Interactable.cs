using UnityEngine;

namespace Gattai.Runtime.Systems.Interactions
{
    public abstract class Interactable : MonoBehaviour
    {
        public abstract InteractionType Type { get; }
        public abstract string Information { get; }
        public abstract void Interact();
    }
}