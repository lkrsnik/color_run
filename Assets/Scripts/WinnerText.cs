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
		winnerText.text = "Winner and King of Rainbow World: \n" + GameManager.instance.players[winnerId].pName + " (Player" + winnerId + "), Area colored: " + ((int)(GameManager.instance.players[winnerId].areaColored*100)).ToString() + "%.\n 2nd: \n"  + GameManager.instance.players[secondID].pName + " (Player" + secondID + "), Area colored: " + ((int)(GameManager.instance.players[secondID].areaColored*100)).ToString() + "%.";
	}
}
