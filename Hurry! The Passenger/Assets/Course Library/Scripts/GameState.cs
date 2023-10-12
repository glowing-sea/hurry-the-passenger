using UnityEngine;
using System.Collections;

public enum GameState
{
    Running, // the player can control the character. The remaining time is decreasing
    Paused, // the player cannot control the character. The remaining time stops decreasing
    Over, // game is finished or failed. The player cannot the character
    LeavingMainScene // the player cannot control the character. The camera is shooting another scene
                     // i.e. the game organising view. The remaining time is decreasing.
}