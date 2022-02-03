using System;
using System.Collections;
using System.Collections.Generic;
using Gattai.Runtime.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gattai.Runtime.Systems.Dialogs
{
    public class DialogWindow : StaticInstance<DialogWindow>
    {
        [Header("Main Components")]
        [SerializeField] private GameObject containerTransform;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI characterNameTextMeshProUGUI;
        [SerializeField] private TextMeshProUGUI dialogTextMeshProUGUI;

        [Header("Buttons")]
        [SerializeField] private Button nextDialogButton;
        [SerializeField] private Button skipDialogButton;
        
        private Queue<string> _sentences;
        private const float SpeedOfText = 0.01f;

        public static event Action OnDialogStarted;
        public static event Action OnDialogEnded;

        private void Start()
        {
            nextDialogButton.onClick.AddListener(NextSentence);
            skipDialogButton.onClick.AddListener(EndDialog);
            _sentences = new Queue<string>();
            containerTransform.SetActive(false);
        }

        public void StartDialog(Dialog dialog)
        {
            OnDialogStarted?.Invoke();

            containerTransform.SetActive(true);
            
            _sentences.Clear();

            foreach (var sentence in dialog.sentences)
            {
                _sentences.Enqueue(sentence);
            }
            
            characterNameTextMeshProUGUI.text = dialog.characterName;
            
            NextSentence();
        }

        private void NextSentence()
        {
            if (_sentences.Count == 0) EndDialog();

            var sentence = _sentences.Dequeue();
            
            StopAllCoroutines();
            StartCoroutine(WriteSentence(sentence));
        }

        private IEnumerator WriteSentence(string sentence)
        {
            dialogTextMeshProUGUI.text = "";

            foreach (var character in sentence.ToCharArray())
            {
                dialogTextMeshProUGUI.text += character;
                yield return new WaitForSeconds(SpeedOfText);
            }
        }

        private void EndDialog()
        {
            containerTransform.SetActive(false);
            
            OnDialogEnded?.Invoke();
        }
    }
}