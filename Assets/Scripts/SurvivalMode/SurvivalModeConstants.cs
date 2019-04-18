using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SurvivalModeConstants
{
    public static readonly float startingTime = 1f;
    public static readonly float decisionTime = 0f;
    public static readonly float animationTime = .25f;
    public static readonly float endingTime = 1f;

    public static readonly int mouseHp = 10;

    public static readonly int miceNumber = 100;
    public static readonly Vector2Int miceStartingPosition = new Vector2Int(0, 0);
}
