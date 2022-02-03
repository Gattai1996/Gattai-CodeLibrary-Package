using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gattai.Runtime.UI
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Mask))]
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollSnapRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

        [Tooltip("Set starting page index - starting from 0")]
        public int startingPage = 0;
        [Tooltip("Threshold time for fast swipe in seconds")]
        public float fastSwipeThresholdTime = 0.3f;
        [Tooltip("Threshold time for fast swipe in (unscaled) pixels")]
        public int fastSwipeThresholdDistance = 100;
        [Tooltip("How fast will page lerp to target position")]
        public float decelerationRate = 10f;
        [Tooltip("Button to go to the previous page (optional)")]
        public Button prevButton;
        [Tooltip("Button to go to the next page (optional)")]
        public Button nextButton;
        [Tooltip("Sprite for unselected page (optional)")]
        public Sprite unselectedPage;
        [Tooltip("Sprite for selected page (optional)")]
        public Sprite selectedPage;
        [Tooltip("Color for selected page icon (optional)")]
        public Color selectedColor;
        [Tooltip("Color for unselected page icon (optional)")]
        public Color unselectedColor;
        [Tooltip("Container with page images (optional)")]
        public Transform pageSelectionIcons;

        // fast swipes should be fast and short. If too long, then it is not fast swipe
        private int _fastSwipeThresholdMaxLimit;

        private ScrollRect _scrollRectComponent;
        private RectTransform _scrollRectTransform;
        private RectTransform _container;

        private bool _horizontal;
    
        // number of pages in container
        private int _pageCount;
        private int _currentPage;

        // whether lerping is in progress and target lerp position
        private bool _lerp;
        private Vector2 _lerpTo;

        // target position of every page
        private List<Vector2> _pagePositions = new List<Vector2>();

        // in draggging, when dragging started and where it started
        private bool _dragging;
        private float _timeStamp;
        private Vector2 _startPosition;

        // for showing small page icons
        private bool _showPageSelection;
        private int _previousPageSelectionIndex;
    
        // container with Image components - one Image for each page
        private List<Image> _pageSelectionImages;

        private void Awake() 
        {
            _scrollRectComponent = GetComponent<ScrollRect>();
            _scrollRectTransform = GetComponent<RectTransform>();
            _container = _scrollRectComponent.content;
            _pageCount = _container.childCount;

            switch (_scrollRectComponent.horizontal)
            {
                // is it horizontal or vertical scrollrect
                case true when !_scrollRectComponent.vertical:
                    _horizontal = true;
                    break;
                case false when _scrollRectComponent.vertical:
                    _horizontal = false;
                    break;
                default:
                    UnityEngine.Debug.LogWarning("Confusing setting of horizontal/vertical direction. Default set to horizontal.");
                    _horizontal = true;
                    break;
            }

            _lerp = false;

            // init
            SetPagePositions();
            SetPage(startingPage);
            InitPageSelection();
            SetPageSelection(startingPage);

            // prev and next buttons
            if (nextButton)
                nextButton.onClick.AddListener(NextScreen);

            if (prevButton)
                prevButton.onClick.AddListener(PreviousScreen);
        }

        private void OnEnable()
        {
            if (_pageCount > 0)
            {
                LerpToPage(startingPage);
            }
        }

        private void Update()
        {
            // if moving to target position
            if (!_lerp) return;
        
            // prevent overshooting with values greater than 1
            var decelerate = Mathf.Min(decelerationRate * Time.deltaTime, 1f);
            _container.anchoredPosition = Vector2.Lerp(_container.anchoredPosition, _lerpTo, decelerate);
        
            // time to stop lerping?
            if (Vector2.SqrMagnitude(_container.anchoredPosition - _lerpTo) < 0.25f) 
            {
                // snap to target and stop lerping
                _container.anchoredPosition = _lerpTo;
                _lerp = false;
                // clear also any scrollrect move that may interfere with our lerping
                _scrollRectComponent.velocity = Vector2.zero;
            }

            // switches selection icon exactly to correct page
            if (_showPageSelection) 
            {
                SetPageSelection(GetNearestPage());
            }
        }

        private void SetPagePositions() 
        {
            var width = 0;
            var height = 0;
            var offsetX = 0;
            var offsetY = 0;
            var containerWidth = 0;
            var containerHeight = 0;

            if (_horizontal) 
            {
                // screen width in pixels of scrollrect window
                width = (int)_scrollRectTransform.rect.width;
                // center position of all pages
                offsetX = width / 2;
                // total width
                containerWidth = width * _pageCount;
                // limit fast swipe length - beyond this length it is fast swipe no more
                _fastSwipeThresholdMaxLimit = width;
            } 
            else 
            {
                height = (int)_scrollRectTransform.rect.height;
                offsetY = height / 2;
                containerHeight = height * _pageCount;
                _fastSwipeThresholdMaxLimit = height;
            }

            // set width of container
            var newSize = new Vector2(containerWidth, containerHeight);
            _container.sizeDelta = newSize;
            var newPosition = new Vector2(containerWidth / 2, containerHeight / 2);
            _container.anchoredPosition = newPosition;

            // delete any previous settings
            _pagePositions.Clear();

            // iterate through all container children and set their positions
            for (var i = 0; i < _pageCount; i++) 
            {
                var child = _container.GetChild(i).GetComponent<RectTransform>();

                var childPosition = _horizontal ? 
                    new Vector2(i * width - containerWidth / 2 + offsetX, 0f) : 
                    new Vector2(0f, -(i * height - containerHeight / 2 + offsetY));
            
                child.anchoredPosition = childPosition;
                _pagePositions.Add(-childPosition);
            }
        }

        private void SetPage(int pageIndex) 
        {
            pageIndex = Mathf.Clamp(pageIndex, 0, _pageCount - 1);
            _container.anchoredPosition = _pagePositions[pageIndex];
            _currentPage = pageIndex;
        }

        private void LerpToPage(int pageIndex) 
        {
            pageIndex = Mathf.Clamp(pageIndex, 0, _pageCount - 1);
            _lerpTo = _pagePositions[pageIndex];
            _lerp = true;
            _currentPage = pageIndex;
        }

        private void InitPageSelection() 
        {
            // page selection - only if defined sprites for selection icons
            _showPageSelection = unselectedPage != null && selectedPage != null;
        
            if (_showPageSelection) 
            {
                // also container with selection images must be defined and must have exactly the same amount of items as pages container
                if (pageSelectionIcons == null || pageSelectionIcons.childCount != _pageCount) 
                {
                    UnityEngine.Debug.LogWarning("Different count of pages and selection icons - will not show page selection");
                    _showPageSelection = false;
                } 
                else 
                {
                    _previousPageSelectionIndex = -1;
                    _pageSelectionImages = new List<Image>();

                    // cache all Image components into list
                    for (var i = 0; i < pageSelectionIcons.childCount; i++) 
                    {
                        var image = pageSelectionIcons.GetChild(i).GetComponent<Image>();
                        var button = pageSelectionIcons.GetChild(i).GetComponent<Button>();
                    
                        if (image == null) 
                        {
                            throw new UnityException("Page selection icon at position " + i + " is missing Image component");
                        }
                    
                        if (button == null) 
                        {
                            throw new UnityException("Page selection icon at position " + i + " is missing Button component");
                        }
                    
                        _pageSelectionImages.Add(image);

                        var pageIndex = i;
                        button.onClick.AddListener(delegate 
                        {
                            LerpToPage(pageIndex);
                        });
                    }
                }
            }

            foreach (var icon in _pageSelectionImages)
            {
                icon.sprite = unselectedPage;
                icon.color = unselectedColor;
            }
        }

        private void SetPageSelection(int aPageIndex) 
        {
            // nothing to change
            if (_previousPageSelectionIndex == aPageIndex) 
            {
                return;
            }
        
            // unselect old
            if (_previousPageSelectionIndex >= 0) 
            {
                _pageSelectionImages[_previousPageSelectionIndex].sprite = unselectedPage;
                _pageSelectionImages[_previousPageSelectionIndex].color = unselectedColor;
                _pageSelectionImages[_previousPageSelectionIndex].transform.localScale = new Vector2(1f, 0.3f);
            }

            // select new
            _pageSelectionImages[aPageIndex].sprite = selectedPage;
            _pageSelectionImages[aPageIndex].color = selectedColor;
            _pageSelectionImages[aPageIndex].transform.localScale = new Vector2(1.25f, 0.3f);

            // Deactivate next and previous buttons if its necessary
            prevButton.interactable = aPageIndex != 0;
            nextButton.interactable = aPageIndex != _pageSelectionImages.Count - 1;

            _previousPageSelectionIndex = aPageIndex;
        }

        private void NextScreen() 
        {
            LerpToPage(_currentPage + 1);
        }

        private void PreviousScreen() 
        {
            LerpToPage(_currentPage - 1);
        }
    
        private int GetNearestPage() 
        {
            // based on distance from current position, find nearest page
            var currentPosition = _container.anchoredPosition;

            var distance = float.MaxValue;
            var nearestPage = _currentPage;

            for (var i = 0; i < _pagePositions.Count; i++) 
            {
                var testDist = Vector2.SqrMagnitude(currentPosition - _pagePositions[i]);

                if (!(testDist < distance)) continue;
            
                distance = testDist;
                nearestPage = i;
            }

            return nearestPage;
        }

        public void OnBeginDrag(PointerEventData aEventData) 
        {
            // if currently lerping, then stop it as user is draging
            _lerp = false;
            // not dragging yet
            _dragging = false;
        }

        public void OnEndDrag(PointerEventData aEventData) {
            // how much was container's content dragged
            float difference;
        
            if (_horizontal) 
            {
                difference = _startPosition.x - _container.anchoredPosition.x;
            } 
            else 
            {
                difference = - (_startPosition.y - _container.anchoredPosition.y);
            }

            // test for fast swipe - swipe that moves only +/-1 item
            if (Time.unscaledTime - _timeStamp < fastSwipeThresholdTime &&
                Mathf.Abs(difference) > fastSwipeThresholdDistance &&
                Mathf.Abs(difference) < _fastSwipeThresholdMaxLimit
            ) // End if
            {
                if (difference > 0) 
                {
                    NextScreen();
                } 
                else 
                {
                    PreviousScreen();
                }
            } 
            else 
            {
                // if not fast time, look to which page we got to
                LerpToPage(GetNearestPage());
            }

            _dragging = false;
        }

        public void OnDrag(PointerEventData aEventData) 
        {
            if (!_dragging) 
            {
                // dragging started
                _dragging = true;
                // save time - unscaled so pausing with Time.scale should not affect it
                _timeStamp = Time.unscaledTime;
                // save current position of cointainer
                _startPosition = _container.anchoredPosition;
            } 
            else 
            {
                if (_showPageSelection) 
                {
                    SetPageSelection(GetNearestPage());
                }
            }
        }
    }
}