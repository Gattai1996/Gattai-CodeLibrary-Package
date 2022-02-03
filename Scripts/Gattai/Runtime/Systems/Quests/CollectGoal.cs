using UnityEngine;

namespace Gattai.Runtime.Systems.Quests
{
    [CreateAssetMenu(menuName = "Quests/Goals/Collect Goal", fileName = "CollectGoal", order = 0)]
    public class CollectGoal : Goal
    {
        public CollectableType collectableType;

        internal void AddProgress(int amount, CollectableType type)
        {
            if (type != collectableType) return;
            
            base.AddProgress(amount);
        }
    }
}
