using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Task", menuName = "Game Data/Player Task")]
public class PlayerTask : ScriptableObject
{
    // Properties
    public string taskName = "";
    [Tooltip("Whether the task is visible in the UI")]
    public bool isVisible = true;

    public State CreateState()
    {
        return new State(this);
    }

    public class State
    {
        public PlayerTask task;
        public bool isComplete = false;

        public State(PlayerTask task)
        {
            this.task = task;
        }
    }
}
