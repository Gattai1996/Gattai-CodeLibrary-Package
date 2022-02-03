using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gattai.Runtime.UI
{
    public class TabGroup : MonoBehaviour
    {
        public Color deselectedColor = new Color(70f, 70f, 70f, 255f);
        public Color selectedColor = Color.white;
        public List<TabButton> tabButtons = new List<TabButton>();
        public List<TextMeshProUGUI> tabButtonsTexts = new List<TextMeshProUGUI>();
        public List<GameObject> gameObjectsToSwap = new List<GameObject>();
        public Button previousTabButton;
        public Button nextTabButton;
        public Color selectedTabTextColor;
        public Color deselectedTabTextColor;
        public TabButton SelectedTabButton { get; private set; }

        private int selectedTabIndex
        {
            get
            {
                for (var i = 0; i < tabButtons.Count; i++)
                {
                    if (tabButtons[i] == SelectedTabButton && SelectedTabButton != null)
                    {
                        return i;
                    }
                }

                return -1;
            }
        }

        private void OnEnable()
        {
            SelectTab(tabButtons[0]);
        }

        private void Start()
        {
            previousTabButton.onClick.AddListener(PreviousTab);
            nextTabButton.onClick.AddListener(NextTab);
            SelectTab(tabButtons[0]);
        }

        public void SelectTab(TabButton tabButton)
        {
            SelectedTabButton = tabButton;
            tabButton.GetComponent<Image>().color = selectedColor;
            tabButton.GetComponentInChildren<TextMeshProUGUI>().color = selectedTabTextColor;
            gameObjectsToSwap[selectedTabIndex].SetActive(true);
            ResetTabs();
            UpdatePreviousAndNextButtons();
        }

        private void ResetTabs()
        {
            foreach (var tabButton in tabButtons)
            {
                if (tabButton == SelectedTabButton && SelectedTabButton != null)
                {
                    continue;
                }

                tabButton.GetComponent<Image>().color = deselectedColor;
                tabButton.GetComponentInChildren<TextMeshProUGUI>().color = deselectedTabTextColor;
            }

            for (var i = 0; i < gameObjectsToSwap.Count; i++)
            {
                if (i == selectedTabIndex && selectedTabIndex != -1)
                {
                    continue;
                }

                gameObjectsToSwap[i].SetActive(false);
            }
        }

        private void UpdatePreviousAndNextButtons()
        {
            if (nextTabButton == null || previousTabButton == null) return;
            previousTabButton.interactable = selectedTabIndex >= 1;
            nextTabButton.interactable = selectedTabIndex <= tabButtons.Count - 2;
        }

        public void PreviousTab()
        {
            var currentSelected = selectedTabIndex - 1;
            SelectTab(tabButtons[currentSelected]);
        }

        public void NextTab()
        {
            var currentSelected = selectedTabIndex + 1;
            SelectTab(tabButtons[currentSelected]);
        }
    }
}