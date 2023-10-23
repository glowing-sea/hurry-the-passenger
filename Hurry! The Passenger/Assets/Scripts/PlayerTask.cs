using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Player Task", menuName = "Game Data/Player Task")]
public class PlayerTask : ScriptableObject, IComparable<PlayerTask>
{
    // Properties
    public string taskName = "";

    public string taskID = "";

    [Tooltip("Whether the task is visible in the UI")]
    public bool isVisible = true;

    [Tooltip("Scene name this task belongs to")]
    public string sceneName = "";

    [Tooltip("Order of which the task appears in the task list")]
    public int sortingOrder = 0;

    public static PlayerTask Get(string path)
    {
        return Resources.Load<PlayerTask>("Player Tasks/" + path);
    }

    public int CompareTo(PlayerTask other)
    {
        // First sort in belongsTo, then sort in sortingOrder
        int sceneNameCompare = sceneName.CompareTo(other.sceneName);
        if (sceneNameCompare != 0) return sceneNameCompare;
        return sortingOrder.CompareTo(other.sortingOrder);
    }

    public struct State
    {
        public bool isComplete;
    }
}
