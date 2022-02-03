using Gattai.Runtime.Systems.Dialogs;
using UnityEngine;

namespace Gattai.Runtime.Systems.Interactions
{
    public class InteractableNpc : Interactable
    {
        [SerializeField] private Dialogs.Dialog dialog;
        public override InteractionType Type => InteractionType.Speakable;
        public override string Information => $"Pressione a tecla \"F\" " +
                                              $"para falar com {dialog.characterName}";

        public override void Interact()
        {
            DialogWindow.Instance.StartDialog(dialog);
        }
    }
}