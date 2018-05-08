using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public static LinkedList<Vector3> SpreadEvenRows(SpreadConfig param)
    {
        LinkedList<Vector3> result = new LinkedList<Vector3>();

        var screen = param.ScreenWidth;

        var z = param.ScreenHeight / 2;
        var xStart = (-param.ScreenWidth / 2) + param.ItemWidth / 2;
        var lastItemX = xStart;
        var rawItems = 0;
        var rawCount = 0;

        for (int i = 0; i < param.ItemsCount; i++)
        {
            var position = new Vector3(lastItemX, 0, z);

            lastItemX += param.ItemWidth + param.ItemMargin;

            result.AddLast(position);

            ++rawItems;

            if (rawItems % param.ItemsPerRaw == 0)
            {
                z += param.ItemMargin;
                if (param.RawShift && rawCount % 2 == 0)
                {
                    lastItemX = xStart + param.ItemWidth;
                } 
                else
                {
                    lastItemX = xStart;
                }
                ++rawCount;
            }
        }
        return result;
    }


}