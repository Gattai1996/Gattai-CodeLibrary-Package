using System;
using UnityEngine;

namespace Gattai.Runtime.Systems.Quests
{
    public abstract class Goal : ScriptableObject
    {
        public string description;
        [SerializeField] private int targetProgress;
        public int TargetProgress => targetProgress;
        
        public int Progress { get; internal set; }
        public bool Completed { get; internal set; }
        
        protected event Action<Goal> OnQuestGoalCompleted;

        protected void AddProgress(int amount)
        {
            Progress += amount;

            if (Progress <= targetProgress) return;
            
            Completed = true;
            OnQuestGoalCompleted?.Invoke(this);
        }
    }
}