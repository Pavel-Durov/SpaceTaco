using UnityEngine;

public class Mover : MonoBehaviour 
{
	public float Speed;

	void Start ()
	{
		var rb = GetComponent<Rigidbody> ();
        rb.velocity = transform.forward * Speed;
	}
}
