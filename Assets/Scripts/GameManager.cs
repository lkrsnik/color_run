using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	//properties
	public string name1;
	public int color1;
	public float area1;

	//public Player[] players;


	// ---------------------------------------------------------------------------------------------------
	// Creates an instance of Gamemanager as a gameobject if an instance does not exist
	// ---------------------------------------------------------------------------------------------------
	public static GameManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject("GameManager").AddComponent<GameManager> ();
			}
			
			return instance;
		}
	} 

	// Sets the instance to null when the application quits
	public void OnApplicationQuit()
	{
		instance = null;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Initialize(){
		area1 = 0.0f;
		name1 = "bla";

	}
}
