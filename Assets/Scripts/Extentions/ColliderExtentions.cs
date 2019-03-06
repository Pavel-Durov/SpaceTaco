public static class ColliderExtentions
{
    public static bool IsPlayer(this UnityEngine.Collider collider)
    {
        return collider.tag == Tags.PLAYER_TAG;
    }

    public static bool IsBoundary(this UnityEngine.Collider collider)
    {

        return collider.tag == Tags.BOUNDARY_TAG;
    }

    public static bool IsShot(this UnityEngine.Collider collider)
    {
        return collider.tag == Tags.SHOT_TAG;
    }

    public static bool IsHazard(this UnityEngine.Collider collider)
    {
        return collider.tag == Tags.HAZARD_TAG;
    }
}
