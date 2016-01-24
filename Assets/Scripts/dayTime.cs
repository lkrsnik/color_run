using UnityEngine;
using System.Collections;

public class dayTime : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.localEulerAngles = new Vector3 (175, 0, 0);
	}

//	// initialPos = 0,70,0
//	// initialRot = 60,0,0
//	// Update is called once per frame
//	void Update () {
//		transform.Rotate(Vector3.right * Time.deltaTime, Space.Self);
//	}

	// initialPos = 0,70,0
	// initialRot = -30,0,0
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.dayNightEnabled)
			transform.Rotate(Vector3.right * Time.deltaTime*1.6f, Space.Self) ;
	}

	public void ResetAngle(Vector3 init){
		transform.localEulerAngles = init;
	}
}
