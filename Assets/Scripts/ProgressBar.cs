using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {
	
	public int ID;
	// Use this for initialization
	void Start () {
	
		 
	}
	
	// Update is called once per frame
	void Update () {
	

//		float area = 0.4f;
		float area = GameManager.instance.players[ID].areaColored;
//		float nArea = GameManager.instance.normalizeArea (area);
		Image image = GetComponent<Image> ();
		image.color = GameManager.instance.players[ID].color;
		image.fillAmount = area;

	}
}