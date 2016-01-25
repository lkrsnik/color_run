using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameLogic gameLogicScript;
	public MenuManager menuScript;
	public dayTime dayTimeScript;

	public float startTime = 2*60;
//	public float startTime = 10;
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
		public float timeSpeedPU;
		public float timeEatPU;
		public float angleDir;
		public bool eatArea;

	};

	//max 4 players
	public pData[] players = new pData[4];
	//number of players
	public int nP;

	public int winnerID;
	public int secondID;

	public bool gameFinished;

//	public int defaultSpeed;

	public float audioVolume;

	public Vector3 lightStartRotation;
	public bool dayNightEnabled;

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
//			Debug.Log ("Player" + i + ": " + p.ToString());
			//p.rigidbody.velocity = Vector3.zero;
			p.transform.position = players [i].initialPos;
			//set material
			SetMaterial(i, players[i].colorID);

			//reset pickup times
			players [i].timeSpeedPU = 0;
			players [i].timeEatPU = 0;

			//reset angle

			players [i].angleDir = 0;
		}

		//reset ground coloring
		gameLogicScript.restartTextures ();

		dayTimeScript.ResetAngle(lightStartRotation);
		if (!dayNightEnabled) {
			dayTimeScript.ResetAngle(new Vector3 (90, 0, 0));
		}

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

		//set player ids

		GameManager.instance.players[0].id = 0;
		GameManager.instance.players[1].id = 1;

		lightStartRotation = new Vector3 (-10, 0, 0);

		//find Menu script
		menuScript = GameObject.FindObjectOfType(typeof(MenuManager)) as MenuManager;
		//find logic script
		gameLogicScript = GameObject.FindObjectOfType(typeof(GameLogic)) as GameLogic;
		//find daytime script
		dayTimeScript = GameObject.FindObjectOfType(typeof(dayTime)) as dayTime;

		audioVolume = 1.0f;

		//reset pickup Times
		for (int i = 0; i < nP; i++) {
			players [i].timeEatPU = 0;
			players [i].timeSpeedPU = 0;
			players [i].eatArea = false;
		}

		dayNightEnabled = true;

	}


	// Update is called once per frame
	void Update () {
		//update timer 
		timerInSeconds -= Time.deltaTime;
		minutes = (int)(timerInSeconds / 60f);
		secondsInMinute = (int)(timerInSeconds % 60f);

		//decrease pickup duration
		for (int i = 0; i < nP; i++) {
			if (players [i].timeSpeedPU > 0) {
				players [i].timeSpeedPU -= Time.deltaTime;
			}
			if (players [i].timeEatPU > 0) {
				players [i].timeEatPU -= Time.deltaTime;
			}
//			Debug.Log ("PickupDuration: " + GameManager.instance.players [i].timeSpeedPU);
		}


		//finish detection
		if (timerInSeconds <= 0 && !gameFinished) {
			winnerID = SelectWinner ();
			menuScript.FinishedGame ();
			gameFinished = true;
			Debug.Log ("Finish");
		}
	}

	Vector3 GetPosition(int playerID){
		GameObject p = GameObject.FindGameObjectWithTag ("Player" + playerID);
//		Debug.Log ("Position: " + p.transform.position);
		return p.transform.position;
	}

	int SelectWinner(){
		float maxArea = 0.0f;
		int wID = 0;
		for (int i = 0; i < nP; i++) {
			if (players[i].areaColored > maxArea) {
				maxArea = players[i].areaColored;
				wID = players[i].id;
			}
//			Debug.Log ("Player" + players[i].id + ", area = " + players[i].areaColored);

		}
		if (wID == 0) {
			secondID = 1;
		} else {
			secondID = 0;
		}
				
//		Debug.Log ("Winner: Player" + GameManager.instance.winnerID + ", area = " + maxArea);
		return wID;
	}


	public void SetMaterial(int playerID, int color){
//		Debug.Log ("setting material for player" + playerID + "to: " + color);

		GameObject p = GameObject.FindGameObjectWithTag ("Player" + playerID);
		Renderer renderer = p.GetComponent<Renderer> ();
		TrailRenderer trenderer = p.GetComponent<TrailRenderer> ();
		Light pLight = p.GetComponent<Light> ();

		switch (color) {
		case 2:
			renderer.material = greenMat;
			trenderer.material = greenTrailMat;
			pLight.color = Color.green;
			break;
		case 3:
			renderer.material = blueMat;
			trenderer.material = blueTrailMat;
			pLight.color = Color.blue;
			break;
		case 4:
			renderer.material = yellowMat;
			trenderer.material = yellowTrailMat;
			pLight.color = Color.yellow;
			break;
		case 5:
			renderer.material = redMat;
			trenderer.material = redTrailMat;
			pLight.color = Color.red;
			break;
		default:
			Debug.Log ("wrong material!");
			break;
		}

	}

	public void SetControllerMode(int mode){
		for (int i = 0; i < nP; i++) {
			GameObject p = GameObject.FindGameObjectWithTag ("Player" + i);
			GameObject[] wallTB = GameObject.FindGameObjectsWithTag ("WallTB");
			GameObject[] wallLR = GameObject.FindGameObjectsWithTag ("WallLR");
			GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("obstacleCube");
			GameObject parentObst = GameObject.FindGameObjectWithTag ("obstacleParent");

//			Debug.Log ("Player" + i + ": " + p.ToString());
			if (mode == 0) {
				p.GetComponent<PlayerController1> ().enabled = true;
				p.GetComponent<PlayerController2> ().enabled = false;
				Debug.Log ("Set mode" + mode + "for: " + p.ToString());

				//set wall behaviour
				for (int j = 0; j < 2; j++) {
					//active for mode0
					wallTB [j].GetComponent<MeshCollider> ().convex = true;
					wallTB [j].GetComponent<MeshCollider> ().isTrigger = true;
					wallLR [j].GetComponent<MeshCollider> ().convex = true;
					wallLR [j].GetComponent<MeshCollider> ().isTrigger = true;
				}
					
				foreach (GameObject obj in obstacles) {
					obj.SetActive (false);
				}


			} else if (mode == 1) {
				p.GetComponent<PlayerController1> ().enabled = false;
				p.GetComponent<PlayerController2> ().enabled = true;
				Debug.Log ("Set mode" + mode + "for: " + p.ToString());

				parentObst.SetActiveRecursively (true);

				//set wall behaviour
				for (int j = 0; j < 2; j++) {
					//active for mode0
//					wallTB [j].GetComponent<MeshCollider> ().convex = false;
					wallTB [j].GetComponent<MeshCollider> ().isTrigger = false;
//					wallLR [j].GetComponent<MeshCollider> ().convex = false;
					wallLR [j].GetComponent<MeshCollider> ().isTrigger = false;
				}


			}
				
		}
	}

}
