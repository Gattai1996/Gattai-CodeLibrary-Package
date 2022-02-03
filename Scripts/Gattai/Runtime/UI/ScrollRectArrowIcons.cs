using UnityEngine;
using UnityEngine.UI;

namespace Gattai.Runtime.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectArrowIcons : MonoBehaviour
    {
        [SerializeField] private GameObject _upArrow = default;
        [SerializeField] private GameObject _downArrow = default;
        [SerializeField, Range(0f, 1f)] private float _minimumScroll = 0.01f;
        [SerializeField, Range(0f, 1f)] private float _maximumScroll = 0.99f;
        private ScrollRect _scrollRect;

        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _scrollRect.onValueChanged.AddListener(delegate { UpdateArrows(); });
        }

        private void OnEnable()
        {
            _upArrow.SetActive(false);
            _downArrow.SetActive(true);
        }

        public void UpdateArrows()
        {
            if (_scrollRect.verticalNormalizedPosition > _minimumScroll)
            {
                _downArrow.SetActive(true);
            }
            else
            {
                _downArrow.SetActive(false);
            }

            if (_scrollRect.verticalNormalizedPosition < _maximumScroll)
            {
                _upArrow.SetActive(true);
            }
            else
            {
                _upArrow.SetActive(false);
            }
        }
    }
}