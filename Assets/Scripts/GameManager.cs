using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	//properties

	public float timerInSeconds; 
	public float minutes, secondsInMinute;


	public int nOfPlayers;

	public string name1;
	public int color1;
	public float area1;

	public Player[] players;


	// Creates an instance of Gamemanager as a gameobject if an instance does not exist
	public static GameManager Instance
	{
		get
		{
			if(instance == null)
			{
//				Component obj = GameObject.GetComponent<GameManager>();
//				instance = new GameObject("GameManagerObj").AddComponent(obj);
				instance = new GameObject("GameManagerObj").AddComponent<GameManager> ();
				instance.GetComponent<GameManager>().enabled = true;
//				GameManager.GetComponentInChildren<GameManagerObj>().enabled = true;

			}
			
			return instance;
		}
	} 

	// Sets the instance to null when the application quits
	public void OnApplicationQuit()
	{
		instance = null;
	}

	void Awake(){

		timerInSeconds = 0.0f;

//		area1 = 0.0f;
//		name1 = "bla";

	}


	public void ResetLevel(){
		timerInSeconds = 0.0f;
		area1 = 0.12345f;
		players [0].name = "p1";
		players [1].name = "p2";
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		timerInSeconds += Time.deltaTime;
		minutes = (int)(timerInSeconds / 60f);
		secondsInMinute = (int)(timerInSeconds % 60f);
	}

}
