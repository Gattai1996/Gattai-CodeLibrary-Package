using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gattai.Runtime.UI
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerClickHandler
    {
        private TabGroup _tabGroup;

        private void Start()
        {
            _tabGroup = transform.parent.GetComponentInParent<TabGroup>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _tabGroup.SelectTab(this);
        }
    }
}