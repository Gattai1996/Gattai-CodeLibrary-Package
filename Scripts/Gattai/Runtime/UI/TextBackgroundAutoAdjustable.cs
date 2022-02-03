using System.Collections;
using TMPro;
using UnityEngine;

namespace Gattai.Runtime.UI
{
    public class TextBackgroundAutoAdjustable : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Vector2 _padding;

        private void OnEnable()
        {
            StartCoroutine(Resize());
        }

        private IEnumerator Resize()
        {
            yield return new WaitForEndOfFrame();
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 size = _text.GetRenderedValues(false);
            size += _padding;
            rectTransform.sizeDelta = size;
        }
    }
}