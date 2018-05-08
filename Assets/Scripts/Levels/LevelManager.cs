using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class LevelManager
{
    public static Level CurrentLevel { get; private set; }
    public static List<Level> Levels;

    public static void Init(GameObject CappaHazard, float x, float y)
    {
        Levels = new List<Level>();

        Levels.Add(new Level(1, new List<Wave>(){
            new Wave(2, 1f, SpreadFactory.EqualSpread(CappaHazard, x, y, 12)),
            new Wave(4, 1f, SpreadFactory.EqualSpread(CappaHazard, x, y, 24)),
            new Wave(5, 1f, SpreadFactory.EqualSpread(CappaHazard, x, y, 24), -7),
        }));

        Levels.Add(new Level(2, new List<Wave>(){
            new Wave(1, 1f, SpreadFactory.ShiftedSpread(6, CappaHazard, x, y, 12)),
            new Wave(2, 1f, SpreadFactory.ShiftedSpread(6, CappaHazard, x, y, 24)),
            new Wave(3, 1f, SpreadFactory.ShiftedSpread(6, CappaHazard, x, y, 48)),
            new Wave(4, 1f, SpreadFactory.ShiftedSpread(6, CappaHazard, x, y, 96)),
        }));

        Levels.Add(new Level(3, new List<Wave>(){
            new Wave(4, 1f, SpreadFactory.ShiftedSpread(6, CappaHazard, x, y, 96), -15),
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
            CurrentLevel = null;
            Debug.Log("NO MORE LEVELS");
        }
    }
}
