using UnityEngine;

namespace Gattai.Runtime.Systems.Dialogs
{
    [CreateAssetMenu(fileName = "New Dialog", menuName = "Create Dialog", order = 1)]
    public class Dialog : ScriptableObject
    {
        public string characterName;
        [TextArea(minLines: 3, maxLines: 20)] public string[] sentences;
    }
}