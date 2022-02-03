using UnityEngine;
using UnityEngine.EventSystems;

namespace Gattai.Runtime.MouseCursor
{
    public class CursorChangeOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private global::Gattai.Runtime.MouseCursor.MouseCursor _mouseCursor;

        private void Awake()
        {
            _mouseCursor = global::Gattai.Runtime.MouseCursor.MouseCursor.Instance;
            _mouseCursor.SetArrowCursor();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _mouseCursor.SetHandCursor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _mouseCursor.SetArrowCursor();
        }

        private void OnDisable()
        {
            _mouseCursor.SetArrowCursor();
        }
    }
}