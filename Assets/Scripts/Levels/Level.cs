using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level
{
    public Level(List<Wave> waves)
    {
        Waves = waves;
    }

    public List<Wave> Waves { get; private set; }
}
