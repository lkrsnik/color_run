using UnityEngine;
using System.Collections;

public class PlayerController1 : MonoBehaviour {
 
	private Rigidbody rb;
	public float angle;
	public int player;
	public float speed = 10;  //default: 10-15

	public float speedUpDuration = 7;
	public float speedUp = 5;


	void Start()
	{
		rb = GetComponent<Rigidbody> ();
		//rb.velocity=new Vector3 (-1.0f, 0.0f, 0.0f) * speed;

	}

	void FixedUpdate()
	{

		float moveH;
		switch (player) {
			case 0:
				moveH = Input.GetAxis ("Horizontal_AD");
				break;
			case 1:
				moveH = Input.GetAxis ("Horizontal_LR");
				break;
			default:
				moveH = 0f;
			break;
		}
		//float moveV = Input.GetAxis ("Vertical_WS");

		//Vector3 movement = Quaternion.AngleAxis(moveH*30, Vector3.up) * new Vector3 (-1.0f, 0.0f, 0.0f);
		angle += moveH * 0.05f;

		rb.velocity=new Vector3 (Mathf.Sin(angle), rb.velocity.y/speed, Mathf.Cos(angle)) * speed;

		//rb.AddForce (Quaternion.AngleAxis (moveH*100, Vector3.up)* rb.velocity);
		//rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other){


		if (other.gameObject.CompareTag ("PickupSpeed")) {
			other.gameObject.SetActive (false);
			Debug.Log ("Picked up Speed");
			GameManager.instance.players [player].timeSpeedPU = speedUpDuration;

			StartCoroutine(SpeedUpBall());
		}

	}

	IEnumerator SpeedUpBall() {
		speed += speedUp;
		Debug.Log ("Speed: " + speed);
		yield return new WaitForSeconds(speedUpDuration);
		speed -= speedUp;
		Debug.Log ("Speed: " + speed);
	}
		
}
