using UnityEngine;

public class UserInputUtil
{
    public static float GetHorizontalMove()
    {
        float delta = Input.GetAxis("Horizontal");
        if (DevicePlatformUtil.IsMobile)
        {
            delta = Input.acceleration.x * 2;
        }
        return delta;
    }

    public static float GetVerticalMove()
    {
        float delta = Input.GetAxis("Vertical");
        if (DevicePlatformUtil.IsMobile)
        {
            delta = Input.acceleration.y > 0 ? Input.acceleration.y * 2 : Input.acceleration.y;
        }
        return delta * 3;
    }

    public static Vector3 GetPlayerMove()
    {
        return new Vector3(GetHorizontalMove(), 0, GetVerticalMove());
    }

    public static bool IsScreenTapped
    {
        get
        {
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
        }
    }

    public static bool IsMouseRightClick
    {
        get
        {
            return Input.GetButton("Fire1");
        }
    }
}
