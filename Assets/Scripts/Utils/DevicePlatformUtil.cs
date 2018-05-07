
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
}
