using UnityEngine;
using System.Collections;

public class SpreadFactory
{
    const int DEFAULT_ITEMS_ROW_COUNT = 6;

    public static SpreadConfig EqualSpread(GameObject gameObj, float x, float y, int count)
    {

        return new SpreadConfig()
        {
            ItemHeight = gameObj.transform.localScale.y,
            ItemWidth = gameObj.transform.localScale.x,
            ScreenWidth = x,
            ScreenHeight = y,
            ItemMargin = gameObj.transform.localScale.x * 2,
            ItemsCount = count,
            ItemsPerRaw = DEFAULT_ITEMS_ROW_COUNT,
            GameObj = gameObj,
            RawShift = false
        };
    }

    public static SpreadConfig ShiftedSpread(int itemsPerRaw, GameObject gameObj, float x, float y, int count)
    {
        var result = EqualSpread(gameObj, x, y, count);
        result.RawShift = true;
        result.ItemsPerRaw = itemsPerRaw;
        return result;
    }
}
