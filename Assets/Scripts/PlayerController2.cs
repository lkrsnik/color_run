using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour {
	
	public float force; //defaul: 15
	private Rigidbody rb;
	public int player;
	public int maxVelocity; //default: 15

	public float angle;

	
	void Start()
	{
		rb = GetComponent<Rigidbody> ();
	}
	
	void FixedUpdate()
	{
		float moveH;
//		float moveV;
		switch (player) {
		case 0:
			moveH = Input.GetAxis ("Horizontal_AD");
//			moveV = Input.GetAxis ("Vertical_WS");
			break;
		case 1:
			moveH = Input.GetAxis ("Horizontal_LR");
//			moveV = Input.GetAxis ("Vertical_UD");
			break;
		default:
			moveH = 0f;
//			moveV = 0f;
			break;
		}


		angle += moveH * 0.04f;

		Vector3 direction = new Vector3 (Mathf.Sin(angle), 0, Mathf.Cos(angle)) ;

		rb.AddForce (direction * force);

		//limit max velocity
		if(rb.velocity.sqrMagnitude > maxVelocity*maxVelocity)
		{
			//smoothness of the slowdown is controlled by the 0.99f, 
			//0.5f is less smooth, 0.9999f is more smooth
			rb.velocity *= 0.7f;
		}
	}
}
