using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	//properties

	public float timerInSeconds; 
	public float minutes, secondsInMinute;


	public int nOfPlayers;

	public string name1;
	public string name2;

	// colors => 0 = blue, 1 = red, 2 = green, 3 = yellow
	public int color1;
	public int color2;

	public float area1;
	public float area2;

//	public PlayerData[] playerDatas = new PlayerData[4];


	// Creates an instance of Gamemanager as a gameobject if an instance does not exist
	public static GameManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject("GameManagerObj").AddComponent<GameManager> ();
				instance.GetComponent<GameManager>().enabled = true;

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

//		playerDA p1 = gameObject.AddComponent<playerDA>;
//		p1.color = 1;


//		PlayerData p1 = gameObject.AddComponent<PlayerData>();
//		PlayerData p2 = gameObject.AddComponent<PlayerData>();
//		playerDatas [0] = p1;
//		playerDatas [1] = p2;
//		playerDatas [0].name = "p1";
//		playerDatas [1].name = "p2";
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

//	public class playerDA{
//		public int id;
//		public int color;
//		public string name;
//		public float areaColored;
//	}

}
