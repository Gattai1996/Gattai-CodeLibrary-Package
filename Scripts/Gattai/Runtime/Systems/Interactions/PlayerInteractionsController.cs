using Gattai.Runtime.Systems.Dialogs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gattai.Runtime.Systems.Interactions
{
    public class PlayerInteractionsController : MonoBehaviour
    {
        private Interactable _interactableInRange;
        private bool _canSpeak = true;

        private void Awake()
        {
            DialogWindow.OnDialogStarted += CannotSpeak;
            DialogWindow.OnDialogEnded += CanSpeak;
        }

        private void OnDestroy()
        {
            DialogWindow.OnDialogStarted -= CannotSpeak;
            DialogWindow.OnDialogEnded -= CanSpeak;
        }
        
        private void CanSpeak()
        {
            _canSpeak = true;
        }
        
        private void CannotSpeak()
        {
            _canSpeak = false;
        }

        private void Update()
        {
            if (_interactableInRange == null) return;
            
            HandleInteraction(_interactableInRange);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<Interactable>(out var interactable))
            {
                _interactableInRange = interactable;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<Interactable>() == _interactableInRange)
            {
                _interactableInRange = null;
            }
        }

        private void HandleInteraction(Interactable interactable)
        {
            switch (interactable.Type)
            {
                case InteractionType.Speakable:
                {
                    if (_canSpeak && Keyboard.current.fKey.wasPressedThisFrame) interactable.Interact();
                    
                    break;
                }
                default:
                {
                    UnityEngine.Debug.LogError($"Interactable '{interactable}' of Type '{interactable.Type}' " +
                                               $"need to be implemented on class {nameof(PlayerInteractionsController)}!");
                    break;
                }
            }
        }
    }
}