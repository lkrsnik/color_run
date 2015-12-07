using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar1 : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
	
		 
	}
	
	// Update is called once per frame
	void Update () {
	

//		float area = 0.4f;
		float area = GameManager.instance.area1;
		float nArea = GameManager.instance.normalizeArea (area);
		Image image = GetComponent<Image> ();
		image.color = GameManager.instance.cColor1;
		image.fillAmount = nArea;

	}
}