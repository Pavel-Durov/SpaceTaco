using System;
using UnityEngine;

public class DevicePlatformUtil : MonoBehaviour
{
    public static bool IsMobile
    {
        get
        {
            return Application.platform == RuntimePlatform.Android ||
                              Application.platform == RuntimePlatform.IPhonePlayer;
        }
    }

    public static void SetScreenOrientation()
    {
        if (IsMobile)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }
}
