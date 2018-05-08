using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelManager
{
    public static Level CurrentLevel { get; private set; }
    public static List<Level> Levels;

    public static void Init(GameObject CappaHazard, float x, float y)
    {
        Levels = new List<Level>();

        Levels.Add(new Level(new List<Wave>(){
            new Wave(5, CappaHazard, x, y, 1),
            new Wave(7, CappaHazard, x, y, 0.5f),
            new Wave(7, CappaHazard, x, y, 1.5f),
        }));

        Levels.Add(new Level(new List<Wave>(){
            new Wave(15, CappaHazard, x, y, 0.1f),
            new Wave(30, CappaHazard, x, y, 0.01f),
        }));

        CurrentLevel = Levels.First();
    }

    public static void LevelEnd()
    {
        var index = Levels.IndexOf(CurrentLevel);
        if (index < Levels.Count - 1)
        {
            CurrentLevel = Levels.ElementAt<Level>(++index);
        }
        else
        {
            Debug.Log("NO MORE LEVELS");
        }
    }
}
