using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gattai.Runtime.Systems.Quests
{
    [CreateAssetMenu(menuName = "Quests/Quest", fileName = "New Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        public string questName;
        [TextArea] public string description;
        public List<string> questRewards;
        public KillEnemiesGoal[] killEnemiesGoals;
        public CollectGoal[] collectGoals;
        public Goal[] AllGoals => killEnemiesGoals.Concat<Goal>(collectGoals).ToArray();
        public bool Completed { get; private set; }
        public event Action OnCompleted;

        private void OnEnable()
        {
            Completed = false;
        }

        private void HandleEnemyOnDestroy(EnemyType enemyType)
        {
            foreach (var goal in killEnemiesGoals.Where(x => x.enemyType == enemyType))
            {
                AddProgress(goal, 1);
            }
        }

        private void HandleCollectableOnCollect(CollectableType collectableType, int amount)
        {
            foreach (var goal in collectGoals.Where(x => x.collectableType == collectableType))
            {
                AddProgress(goal, amount);
            }
        }

        private void AddProgress(KillEnemiesGoal goal, int amount)
        {
            goal.AddProgress(amount, goal.enemyType);
            CheckCompleted();
        }

        private void AddProgress(CollectGoal goal, int amount)
        {
            goal.AddProgress(amount, goal.collectableType);
            CheckCompleted();
        }

        private void CheckCompleted()
        {
            if (killEnemiesGoals.Any(x => !x.Completed)) return;
            if (collectGoals.Any(x => !x.Completed)) return;
            Complete();
        }
        
        private void Complete()
        {
            Completed = true;
            OnCompleted?.Invoke();
            GrantRewards();
        }

        private void GrantRewards()
        {
            foreach (var reward in questRewards)
            {
                UnityEngine.Debug.Log("Reward granted: " + reward);
            }
        }
    }
}