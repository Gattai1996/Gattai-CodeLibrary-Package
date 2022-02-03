using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gattai.Runtime.UI
{
    /// <summary>
    /// Show a Modal Window to the user and can be configured to do various actions.
    /// It can be used to show information with dialog windows, question windows, images, etc.
    /// Access with ModalWindow.Singleton and invoke the ShowModalWindow method.
    /// </summary>
    public class ModalWindow : MonoBehaviour
    {
        #region Variables
        
        /// <summary>
        /// The singleton instance of the Modal Window.
        /// </summary>
        public static ModalWindow Instance;
    
        [Header("Background")]
        [SerializeField] private GameObject backgroundGameObject = default;
    
        [Header("Panels")]
        [SerializeField] private GameObject headerPanel = default;
        [SerializeField] private GameObject contentPanel = default;
        [SerializeField] private GameObject footerPanel = default;
    
        [Header("Layouts")]
        [SerializeField] private GameObject horizontalLayout = default;
        [SerializeField] private GameObject verticalLayout = default;
    
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI headerText = default;
        [SerializeField] private TextMeshProUGUI horizontalLayoutContentText = default;
        [SerializeField] private TextMeshProUGUI verticalLayoutContentText = default;
        [SerializeField] private TextMeshProUGUI confirmButtonText = default;
        [SerializeField] private TextMeshProUGUI cancelButtonText = default;
        [SerializeField] private TextMeshProUGUI alternativeButtonText = default;
        [SerializeField] private TextMeshProUGUI toggleText = default;
    
        [Header("Images")]
        [SerializeField] private Image horizontalLayoutImage = default;
        [SerializeField] private Image verticalLayoutImage = default;
    
        [Header("Buttons")]
        [SerializeField] private Button confirmButton = default;
        [SerializeField] private Button cancelButton = default;
        [SerializeField] private Button alternativeButton = default;
        [SerializeField] private Toggle toggle = default;
    
        private Button _backgroundButton;
        private Action _confirmAction;
        private Action _cancelAction;
        private Action _alternativeAction;
        private Action _toggleActionTrue;
        private Action _toggleActionFalse;
        private readonly Action _emptyAction = () => { };
        
        #endregion

        #region Initial Setup

        private void Awake()
        {
            Instance = this;
            SetButtons();
            backgroundGameObject.SetActive(false);
        }
        
        private void SetButtons()
        {
            _backgroundButton = backgroundGameObject.GetComponent<Button>();
            _backgroundButton.onClick.AddListener(() => { backgroundGameObject.SetActive(false); });
            confirmButton.onClick.AddListener(HandleConfirm);
            cancelButton.onClick.AddListener(HandleCancel);
            alternativeButton.onClick.AddListener(HandleAlternative);
        }

        #endregion

        #region Show Dialog Window

        /// <summary>
        /// Show a simple Dialog Window with a message.
        /// </summary>
        /// <param name="text">The text of the message.</param>
        /// <param name="buttonCancelText">The text of the close button.</param>
        public void ShowDialogueWindow(string text, string buttonCancelText)
        {
            SetDialogWindowActivations();
            SetDialogWindowTexts(text, buttonCancelText);
            _cancelAction = _emptyAction;
            confirmButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// Show a simple Dialog Window with a message.
        /// </summary>
        /// <param name="text">The text of the message.</param>
        /// <param name="buttonCancelText">The text of the close button.</param>
        /// <param name="closeButtonAction">Action performed by the close button.</param>
        public void ShowDialogueWindow(string text, string buttonCancelText, Action closeButtonAction)
        {
            SetDialogWindowActivations();
            SetDialogWindowTexts(text, buttonCancelText);
            _cancelAction = closeButtonAction;
            confirmButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// Show a question with 2 alternatives, confirm and cancel.
        /// </summary>
        /// <param name="text">The text of the message.</param>
        /// <param name="buttonConfirmText">The text of the confirm button.</param>
        /// <param name="buttonCancelButtonText">The text of the cancel button.</param>
        /// <param name="confirmAction">The logic that the confirm button will execute.</param>
        public void ShowDialogueWindow(string text, string buttonConfirmText, string buttonCancelButtonText, 
            Action confirmAction)
        {
            SetDialogWindowActivations();
            SetDialogWindowTexts(text, buttonCancelButtonText, buttonConfirmText);
            SetActions(confirmAction);
        }

        /// <summary>
        /// Show a question with 2 alternatives, confirm and cancel, with a toggle option.
        /// </summary>
        /// <param name="text">The text of the message.</param>
        /// <param name="buttonConfirmText">The text of the confirm button.</param>
        /// <param name="buttonCancelButtonText">The text of the cancel button.</param>
        /// <param name="toggleLabelText">The text of the toggle label.</param>
        /// <param name="confirmButtonAction">The logic that the confirm button will execute.</param>
        /// <param name="toggleActionTrue">The logic that the toggle will execute when it is on.</param>
        /// <param name="toggleActionFalse">The logic that the toggle will execute when it is off.</param>
        public void ShowDialogueWindow(string text, string buttonConfirmText, string buttonCancelButtonText, 
            string toggleLabelText, Action confirmButtonAction, Action toggleActionTrue, Action toggleActionFalse)
        {
            SetDialogWindowActivations(true);
            SetDialogWindowTexts(text, buttonCancelButtonText, buttonConfirmText, toggleLabelText);
            SetActions(confirmButtonAction, toggleActionTrue, toggleActionFalse);
        }

        #endregion

        #region Dialog Window Setup

        private void SetDialogWindowActivations(bool showToggle = false)
        {
            backgroundGameObject.SetActive(true);
            contentPanel.SetActive(false);
            alternativeButton.gameObject.SetActive(false);
            confirmButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
            toggle.gameObject.SetActive(showToggle);
        }

        private void SetDialogWindowTexts(string text, string cancelText, 
            string confirmText = "", string toggleLabelText = "")
        {
            headerText.SetText(text);
            cancelButtonText.SetText(cancelText);
            confirmButtonText.SetText(confirmText);
            toggleText.SetText(toggleLabelText);
        }

        private void SetActions(Action confirmAction, Action toggleActionTrue = null, 
            Action toggleActionFalse = null, Action cancelAction = null)
        {
            _confirmAction = confirmAction;
            _toggleActionTrue = toggleActionTrue ?? _emptyAction;
            _toggleActionFalse = toggleActionFalse ?? _emptyAction;
            _cancelAction = cancelAction ?? _emptyAction;
        }

        #endregion

        #region Buttons Methods
        
        public void HandleConfirm()
        {
            InvokeToggleActions();
            _confirmAction?.Invoke();
            backgroundGameObject.SetActive(false);
        }

        public void HandleCancel()
        {
            InvokeToggleActions();
            _cancelAction?.Invoke();
            backgroundGameObject.SetActive(false);
        }

        public void HandleAlternative()
        {
            InvokeToggleActions();
            _alternativeAction?.Invoke();
            backgroundGameObject.SetActive(false);
        }

        private void InvokeToggleActions()
        {
            if (!toggle.gameObject.activeInHierarchy) return;
            
            if (toggle.isOn)
            {
                _toggleActionTrue?.Invoke();
            }
            else
            {
                _toggleActionFalse?.Invoke();
            }
        }

        #endregion
    }
}