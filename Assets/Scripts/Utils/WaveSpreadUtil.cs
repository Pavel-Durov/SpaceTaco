using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpreadConfig
{
    public int ItemsCount { get; set; }
    public float ItemWidth { get; set; }
    public float ItemHeight { get; set; }
    public float ItemMargin { get; set; }
    public float ScreenWidth { get; set; }
    public float ScreenHeight { get; set; }
    public float ItemsPerRaw { get; set; }
    public bool RawShift { get; set; }
    public GameObject GameObj { get; set; }
}

public class WaveSpreadUtil
{
    public static List<Vector3> CalcSpread(SpreadConfig config)
    {
        List<Vector3> result = new List<Vector3>();

        float lastZ = CalcStartZ(config);
        float lastItemX = CalcStartX(config);
        int itemsInRaw = 0;
        int rawCount = 0;

        foreach (var index in Enumerable.Range(0, config.ItemsCount))
        {
            ++itemsInRaw;

            result.Add(new Vector3(lastItemX, 0, lastZ));
            lastItemX = CalcNextX(lastItemX, config);

            if (IsEndOfLine(itemsInRaw, config))
            {
                lastZ = CalcNextZ(lastZ, config);

                if (IsRowShift(rawCount, config))
                {
                    lastItemX = CalcStartXWithShift(config);
                }
                else
                {
                    lastItemX = CalcStartX(config);
                }
                ++rawCount;
            }
        }

        return result;
    }

    private static bool IsEndOfLine(int itemsInRaw, SpreadConfig config)
    {
        return itemsInRaw % config.ItemsPerRaw == 0;
    }

    private static bool IsRowShift(int rawCount, SpreadConfig config)
    {
        return config.RawShift && rawCount % 2 == 0;
    }

    private static float CalcStartX(SpreadConfig config)
    {
        return (-config.ScreenWidth / 2) + config.ItemWidth / 2;
    }

    private static float CalcStartXWithShift(SpreadConfig config)
    {
        return CalcStartX(config) + config.ItemWidth;
    }

    private static float CalcNextX(float currentX, SpreadConfig config)
    {
        return currentX + config.ItemWidth + config.ItemMargin;
    }

    private static float CalcNextZ(float currentZ, SpreadConfig config)
    {
        return currentZ += config.ItemMargin;
    }

    private static float CalcStartZ(SpreadConfig config)
    {
        return config.ScreenHeight / 2; ;
    }


}