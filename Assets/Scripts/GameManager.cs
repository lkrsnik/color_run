﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameLogic gameLogicScript;
	public MenuManager menuScript;

//	public float startTime = 2*60;
	public float startTime = 10;
	public float timerInSeconds; 
	public float minutes, secondsInMinute;

	Material blueMat;
	Material redMat;
	Material greenMat;
	Material yellowMat;

	Material blueTrailMat;
	Material redTrailMat;
	Material greenTrailMat;
	Material yellowTrailMat;

	//player data
	public struct pData{
		public int id;
		// colors => 2 = green, 3 = blue, 4 = yellow, 5 = red
		public int colorID;
		public Color color;
		public string pName;
		public float areaColored;
		public Vector3 initialPos;

	};

	//max 4 players
	public pData[] players = new pData[4];
	//number of players
	public int nP;

	public int winnerID;
	public int secondID;

	bool gameFinished;

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
		timerInSeconds = startTime;
	}
		

	public void ResetLevel(){
		Debug.Log ("Reset Level");
		timerInSeconds = startTime;
		Time.timeScale = 1;
		gameFinished = false;

		for (int i = 0; i < nP; i++) {
			//reset area
			players [i].areaColored = 0.0f;
			//reset position
			GameObject p = GameObject.FindGameObjectWithTag ("Player" + i);
			Debug.Log ("Player" + i + ": " + p.ToString());
			//p.rigidbody.velocity = Vector3.zero;
			p.transform.position = players [i].initialPos;
			//set material
			SetMaterial(i, players[i].colorID);
		}

		//reset ground coloring
		gameLogicScript = GameObject.FindObjectOfType(typeof(GameLogic)) as GameLogic;
		gameLogicScript.restartTextures ();

	}

	// Use this for initialization
	void Start () {

		nP = 2;
		// Assigns a material named "Assets/Resources/blueSmiley" to the object.
		blueMat = Resources.Load("blueSmiley", typeof(Material)) as Material;
		redMat = Resources.Load("redSmiley", typeof(Material)) as Material;
		greenMat = Resources.Load("greenSmiley", typeof(Material)) as Material;
		yellowMat = Resources.Load("yellowSmiley", typeof(Material)) as Material;

		blueTrailMat = Resources.Load("blueTrail", typeof(Material)) as Material;
		redTrailMat = Resources.Load("redTrail", typeof(Material)) as Material;
		greenTrailMat = Resources.Load("greenTrail", typeof(Material)) as Material;
		yellowTrailMat = Resources.Load("yellowTrail", typeof(Material)) as Material;

		//get the start-position
		players[1].initialPos = GetPosition (1);
		players[0].initialPos = GetPosition (0);
		//pause Game
		Time.timeScale = 0;
		gameFinished = false;

		//find Menu script
		menuScript = GameObject.FindObjectOfType(typeof(MenuManager)) as MenuManager;

	}


	// Update is called once per frame
	void Update () {
		//update timer 
		timerInSeconds -= Time.deltaTime;
		minutes = (int)(timerInSeconds / 60f);
		secondsInMinute = (int)(timerInSeconds % 60f);

		if (timerInSeconds <= 0 && !gameFinished) {
			winnerID = SelectWinner ();
			menuScript.FinishedGame ();
			gameFinished = true;
			Debug.Log ("Finish");
		}
	}

	Vector3 GetPosition(int playerID){
		GameObject p = GameObject.FindGameObjectWithTag ("Player" + playerID);
		Debug.Log ("Position: " + p.transform.position);
		return p.transform.position;
	}

	int SelectWinner(){
		float maxArea = 0.0f;
		int wID = 0;
		foreach (pData player in players) {
			if (player.areaColored > maxArea) {
				maxArea = player.areaColored;
				wID = player.id;
			}
		}
		if (wID == 0)
			secondID = 1;
		else
			secondID = 0;
		
		Debug.Log ("Winner: Player" + winnerID + ", area = " + maxArea);
		return wID;
	}

//	public float normalizeArea(float a){
//		float aMin = 0.0f;
//		float aMax = 60 * 60;
//
//		float normalized = (a - aMin) / (aMax - aMin);
//
//		return normalized;
//
//	}

	public void SetMaterial(int playerID, int color){
		Debug.Log ("setting material for player" + playerID + "to: " + color);

		GameObject p = GameObject.FindGameObjectWithTag ("Player" + playerID);
		Renderer renderer = p.GetComponent<Renderer> ();
		TrailRenderer trenderer = p.GetComponent<TrailRenderer> ();

		switch (color) {
		case 2:
			renderer.material = greenMat;
			trenderer.material = greenTrailMat;
			break;
		case 3:
			renderer.material = blueMat;
			trenderer.material = blueTrailMat;
			break;
		case 4:
			renderer.material = yellowMat;
			trenderer.material = yellowTrailMat;
			break;
		case 5:
			renderer.material = redMat;
			trenderer.material = redTrailMat;
			break;
		default:
			Debug.Log ("wrong material!");
			break;
		}

	}

}
