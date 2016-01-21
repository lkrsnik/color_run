using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerName1 : MonoBehaviour {

	Text playerText;

	// Use this for initialization
	void Update () {
		playerText = GetComponent<Text> () as Text;
		playerText.text = GameManager.instance.name1;
		playerText.color = GameManager.instance.cColor1;
	}

}
