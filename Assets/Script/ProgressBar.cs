using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	public float progress = 0.3f;

	// Use this for initialization
	void Start () {
	
		 
	}
	
	// Update is called once per frame
	void Update () {
	
		Image image = GetComponent<Image> ();
		image.fillAmount = progress;

	}
}
