using UnityEngine;
using System.Collections;

public class PlayerController1 : MonoBehaviour {
 
	private Rigidbody rb;
//	public float angle;
	public int player;
	public float speed = 10;  //default: 10-15

	public float speedUpDuration = 7;
	public float speedUp = 5;

	public float eatDuration = 10;

	float defaultSpeed;
	float fasterSpeed;


	void Start()
	{
		rb = GetComponent<Rigidbody> ();
		//rb.velocity=new Vector3 (-1.0f, 0.0f, 0.0f) * speed;


		defaultSpeed = speed;
		fasterSpeed = speed + speedUp;
//		angle = 0;
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
		GameManager.instance.players [player].angleDir += moveH * 0.05f;

		rb.velocity=new Vector3 (Mathf.Sin(GameManager.instance.players [player].angleDir), rb.velocity.y/speed, Mathf.Cos(GameManager.instance.players [player].angleDir)) * speed;

		//rb.AddForce (Quaternion.AngleAxis (moveH*100, Vector3.up)* rb.velocity);
		//rb.AddForce (movement * speed);

		if (GameManager.instance.players [player].timeEatPU > 0) {
			Debug.Log ("Eating area active for player " + player);
			//TODO: activate eating for each player

		} else {
			Debug.Log ("Eating area inactive for player " + player);

		}

		if (GameManager.instance.players [player].timeSpeedPU > 0) {
			Debug.Log ("Speed-UP active for player " + player);
			speed = fasterSpeed;
			Debug.Log ("Speed: " + speed);

		} else {
			Debug.Log ("Speed-UP inactive for player " + player);
			speed = defaultSpeed;
			Debug.Log ("Speed: " + speed);
		}
	}

	void OnTriggerEnter(Collider other){


		if (other.gameObject.CompareTag ("PickupSpeed")) {
			other.gameObject.SetActive (false);
			Debug.Log ("Picked up Speed");
			GameManager.instance.players [player].timeSpeedPU += speedUpDuration;

//			StartCoroutine (SpeedUpBall ());
		} 
		else if (other.gameObject.CompareTag ("PickupEat")) {
			other.gameObject.SetActive (false);
			Debug.Log ("Picked up Eatpickup");
			GameManager.instance.players [player].timeEatPU += eatDuration;

			//			StartCoroutine(eatArea());
		}

		else if (other.gameObject.CompareTag ("WallLR")){
			GameManager.instance.players [player].angleDir = GameManager.instance.players [player].angleDir * (-1);
			//Debug.Log ("Yahhoooooooo! " + other.gameObject.name);
		} else if (other.gameObject.CompareTag ("WallTB")){
			GameManager.instance.players [player].angleDir = (Mathf.PI) - GameManager.instance.players [player].angleDir;
			//Debug.Log ("Yahhoooooooo! " + other.gameObject.name);
		}

	}

//	IEnumerator SpeedUpBall() {
//		speed += speedUp;
//		Debug.Log ("Speed: " + speed);
//		yield return new WaitForSeconds(speedUpDuration);
//		speed -= speedUp;
//		Debug.Log ("Speed: " + speed);
//	}
		
}
