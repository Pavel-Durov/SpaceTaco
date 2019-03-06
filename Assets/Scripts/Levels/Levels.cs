using UnityEngine;
using System.Collections.Generic;

public class Levels
{
    public static List<Level> _levels;
    private static IEnumerator<Level> _enumerator;

    public static void Init(GameObject CappaHazard, float x, float y)
    {
        _levels = new List<Level>();

        _levels.Add(new Level(1, new List<Wave>(){
            new Wave(1, SpreadFactory.EqualSpread(CappaHazard, x, y, 12)),
            new Wave(2, SpreadFactory.EqualSpread(CappaHazard, x, y, 24)),
            new Wave(3, SpreadFactory.EqualSpread(CappaHazard, x, y, 24), -7),
        }));

        _levels.Add(new Level(2, new List<Wave>(){
            new Wave(1, SpreadFactory.ShiftedSpread(6, CappaHazard, x, y, 12)),
            new Wave(2, SpreadFactory.ShiftedSpread(6, CappaHazard, x, y, 24)),
            new Wave(3, SpreadFactory.ShiftedSpread(6, CappaHazard, x, y, 48)),
            new Wave(4, SpreadFactory.ShiftedSpread(6, CappaHazard, x, y, 96)),
        }));

        _levels.Add(new Level(3, new List<Wave>(){
            new Wave(1, SpreadFactory.ShiftedSpread(6, CappaHazard, x, y, 96), -15),
        }));

        _enumerator = _levels.GetEnumerator();
    }

    public static Level NextLevel()
    {
        Level level = null;
        if (_enumerator.MoveNext())
        {
            level = _enumerator.Current;
        }
        return level;
    }
}
