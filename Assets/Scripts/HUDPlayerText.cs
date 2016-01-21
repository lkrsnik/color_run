using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDPlayerText : MonoBehaviour {

	public int playerID;
	Text playerText;

	// Use this for initialization
	void Update () {
		playerText = GetComponent<Text> () as Text;
		playerText.text = GameManager.instance.players[playerID].pName;
		playerText.color = GameManager.instance.players[playerID].color;
	}

}
