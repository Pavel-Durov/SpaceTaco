using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if(!other.IsPlayer())
        {
            Destroy(other.gameObject);
        }
    }
}
