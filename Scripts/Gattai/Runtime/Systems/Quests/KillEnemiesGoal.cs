using UnityEngine;

namespace Gattai.Runtime.Systems.Quests
{
    [CreateAssetMenu(menuName = "Quests/Goals/Kill Enemies Goal", fileName = "New Kill Enemies Goal", order = 0)]
    public class KillEnemiesGoal : Goal
    {
        public EnemyType enemyType;

        internal void AddProgress(int amount, EnemyType type)
        {
            if (type != enemyType) return;
            
            base.AddProgress(amount);
        }
    }
}
