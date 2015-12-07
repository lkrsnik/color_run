using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar2 : MonoBehaviour {
	
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {

		float area = GameManager.instance.area2;
		float nArea = GameManager.instance.normalizeArea (area);
		Image image = GetComponent<Image> ();
		image.fillAmount = nArea;
		
	}
}