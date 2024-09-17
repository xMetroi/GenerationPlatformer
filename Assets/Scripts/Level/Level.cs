using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public string levelName;
    public float maxTimeToPerfect;
    public int initialCountdown;

    public Level(string levelName, float maxTimeToPerfect, int initialCountdown)
    {
        this.levelName = levelName;
        this.maxTimeToPerfect = maxTimeToPerfect;
        this.initialCountdown = initialCountdown;
    }
}
