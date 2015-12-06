using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int id;
	public int color;
	public string name;
	public float areaColored;

	void Awake(){
		areaColored = 0.0f;
		name = "p1";
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
