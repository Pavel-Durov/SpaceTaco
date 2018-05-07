public static class ColliderExtentions
{
    public const string BOUNDARY_TAG = "Boundary";
    public const string PLAYER_TAG = "Player";
    public const string SHOT_TAG = "Shot";
    public const string HAZARD_TAG = "Hazard";

    public static bool IsPlayer(this UnityEngine.Collider collider){
        return collider.tag == PLAYER_TAG;
    }

    public static bool IsBoundary(this UnityEngine.Collider collider)
    {
        return collider.tag == BOUNDARY_TAG;
    }

    public static bool IsShot(this UnityEngine.Collider collider)
    {
        return collider.tag == SHOT_TAG;
    }

    public static bool IsHazard(this UnityEngine.Collider collider)
    {
        return collider.tag == HAZARD_TAG;
    }
}
