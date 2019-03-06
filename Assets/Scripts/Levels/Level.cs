using System.Collections.Generic;

public class Level
{
    public Level(int number, List<Wave> waves)
    {
        Number = number;
        Waves = waves;
    }
    public int Number{get; private set;}

    public List<Wave> { Waves { get; priv ate set; }
}
