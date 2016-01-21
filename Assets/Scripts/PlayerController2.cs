using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour {
	
	public float speed;
	private Rigidbody rb;

	
	void Start()
	{
		rb = GetComponent<Rigidbody> ();
	}
	
	void FixedUpdate()
	{
		float moveH = Input.GetAxis ("Horizontal_LR");
		float moveV = Input.GetAxis ("Vertical_UD");
		
		Vector3 movement = new Vector3 (moveH, 0.0f, moveV);
		
		rb.AddForce (movement * speed);
	}
}
