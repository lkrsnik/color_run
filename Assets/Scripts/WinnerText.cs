using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinnerText : MonoBehaviour {

	public int winnerId;
	public int secondID;
	Text winnerText;

	void Start () {
		winnerText = GetComponent<Text> () as Text;
	}

	// Use this for initialization
	void Update () {

		winnerId = GameManager.instance.winnerID;
		secondID = GameManager.instance.secondID;
//		Debug.Log ("WinnerTextID: " + winnerId);
		winnerText.text = "Winner: \n" + GameManager.instance.players[winnerId].pName + " (Player" + winnerId + "), Area colored: " + GameManager.instance.players[winnerId].areaColored + ".\n 2nd: "  + GameManager.instance.players[secondID].pName + " (Player" + secondID + "), Area colored: " + GameManager.instance.players[secondID].areaColored + ".";
	}
}
