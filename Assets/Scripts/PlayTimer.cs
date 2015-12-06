using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayTimer : MonoBehaviour {

//	public float seconds, minutes;

	Text counterText;


	// Use this for initialization
	void Start () {
		counterText = GetComponent<Text> () as Text;
	}

	void Update(){
//		minutes = (int)(Time.time / 60f);
//		seconds = (int)(Time.time % 60f);
		counterText.text =  GameManager.instance.minutes.ToString("00") + ":" + GameManager.instance.secondsInMinute.ToString("00");
	}

}
