using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {
	
	public int ID;
	
	// Update is called once per frame
	void Update () {
		float area = GameManager.instance.players[ID].areaColored;
		float nArea = area*2;
		Image image = GetComponent<Image> ();
		image.color = GameManager.instance.players[ID].color;
		image.fillAmount = nArea;

	}
}