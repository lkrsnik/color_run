using UnityEngine;
using System.Collections;

public class StartFromLevel1 : MonoBehaviour {

	//only activate this to start not from the menu !

	// Use this for initialization
	void Start () {
		//setting up GameManager
		DontDestroyOnLoad(GameManager.Instance);
		GameManager.instance.ResetLevel ();
	}

}
