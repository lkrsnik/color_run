using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayTimer : MonoBehaviour {

//	public float seconds, minutes;

	public int type;
	Text counterText;


	// Use this for initialization
	void Start () {
		counterText = GetComponent<Text> () as Text;
	}

	void Update(){
//		minutes = (int)(Time.time / 60f);
//		seconds = (int)(Time.time % 60f);

		counterText.text = "";
		switch (type) {
		//normal timer
		case 0:
			counterText.text = GameManager.instance.minutes.ToString ("00") + ":" + GameManager.instance.secondsInMinute.ToString ("00");
			break;
		// P0 speedup
		case 1:
			if (GameManager.instance.players [0].timeSpeedPU > 0) {
				GetComponentInParent<Image> ().enabled = true;
				counterText.text = "Speed-UP: " + GameManager.instance.players [0].timeSpeedPU.ToString ("0:00");
			} else {
				counterText.text = "";
				GetComponentInParent<Image> ().enabled = false;
			}	
			break;
		// P0 other powerup
		case 2:
			if (GameManager.instance.players [0].timeEatPU > 0) {
				GetComponentInParent<Image> ().enabled = true;
				counterText.text = "Eating: " + GameManager.instance.players [0].timeEatPU.ToString ("0:00");
			} else {
				counterText.text = "";
				GetComponentInParent<Image> ().enabled = false;
			}	
			break;
		// P1 other powerup
		case 3:
			if (GameManager.instance.players [1].timeEatPU > 0) {
				GetComponentInParent<Image> ().enabled = true;
				counterText.text = "Eating: " + GameManager.instance.players [1].timeEatPU.ToString ("0:00");
			} else {
				counterText.text = "";
				GetComponentInParent<Image> ().enabled = false;
			}	
			break;

		// P1 speedup
		case 4:
			if (GameManager.instance.players [1].timeSpeedPU > 0) {
				GetComponentInParent<Image> ().enabled = true;
				counterText.text = "Speed-UP: " + GameManager.instance.players [1].timeSpeedPU.ToString ("0:00");
			} else {
				counterText.text = "";
				GetComponentInParent<Image> ().enabled = false;
			}	
			break;

		default:
			Debug.Log ("wrong case!");
			break;
				
		}
			

	}

}
