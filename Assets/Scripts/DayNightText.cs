using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayNightText : MonoBehaviour {

	Text textDN;

	void Start () {
		textDN = GetComponent<Text> () as Text;
	}

	
	// Update is called once per frame
	void Update () {
		textDN.text = "Day-Night-Cycle enabled: " + GameManager.instance.dayNightEnabled;
	}
}
