using Gattai.Runtime.Singletons;
using UnityEngine;

namespace Gattai.Runtime.MouseCursor
{
    [CreateAssetMenu(menuName = "Create MouseCursor", fileName = "MouseCursor", order = 0)]
    public class MouseCursor : ScriptableObjectSingleton<MouseCursor>
    {
        public Texture2D pointerArrowTexture2D;
        public Texture2D pointerHandTexture2D;

        private void OnEnable()
        {
            SetArrowCursor();
        }

        public void SetArrowCursor()
        {
            Cursor.SetCursor(pointerArrowTexture2D, Vector2.zero, CursorMode.ForceSoftware);
        }

        public void SetHandCursor()
        {
            Cursor.SetCursor(pointerHandTexture2D, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}