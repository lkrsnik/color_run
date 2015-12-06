using UnityEngine;
using System.Collections;

public class PlayerController1 : MonoBehaviour {

	public float speed;
	private Rigidbody rb;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate()
	{
		float moveH = Input.GetAxis ("Horizontal_AD");
		float moveV = Input.GetAxis ("Vertical_WS");

		Vector3 movement = new Vector3 (moveH, 0.0f, moveV);

		rb.AddForce (movement * speed);
	}
}
