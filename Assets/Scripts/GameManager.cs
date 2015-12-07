using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	//properties

	public float startTime = 2*60;
	public float timerInSeconds; 
	public float minutes, secondsInMinute;


	public int nOfPlayers;

	public string name1;
	public string name2;

	// colors => 0 = blue, 1 = red, 2 = green, 3 = yellow
	public int color1;
	public Color cColor1;
	public int color2;
	public Color cColor2;
	
	public float area1;
	public float area2;

	// Assigns a material named "Assets/Resources/blueSmiley" to the object.
	Material blueMat = Resources.Load("blueSmiley", typeof(Material)) as Material;
	Material redMat = Resources.Load("redSmiley", typeof(Material)) as Material;
	Material greenMat = Resources.Load("greenSmiley", typeof(Material)) as Material;
	Material yellowMat = Resources.Load("yellowSmiley", typeof(Material)) as Material;

	Material blueTrailMat = Resources.Load("blueTrail", typeof(Material)) as Material;
	Material redTrailMat = Resources.Load("redTrail", typeof(Material)) as Material;
	Material greenTrailMat = Resources.Load("greenTrail", typeof(Material)) as Material;
	Material yellowTrailMat = Resources.Load("yellowTrail", typeof(Material)) as Material;


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

		timerInSeconds = startTime;

	}

//	public static GameObject p1;

	public void ResetLevel(){
		timerInSeconds = startTime;
		area1 = 0.0f;
		area2 = 0.0f;

		//setup material
		Debug.Log ("Color1: " + color1);
		Debug.Log ("Color2: " + color2);


		//get sphere objects
		//Debug.Log (GameObject.FindGameObjectWithTag ("Player1"));
//		Debug.Log (GameObject.Find("Player1"));

//		p1 = GameObject.FindGameObjectWithTag ("Player1");
//		p1 = GameObject.Find ("Player1");
//		Debug.Log ("P1: " + p1.ToString());

//		Renderer rend = p1.GetComponent<Renderer> ();
//		rend.material = redMat;

		//p1.renderer.material = blueMat;


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

	bool set = false;
	// Update is called once per frame
	void Update () {
		//update timer 
		timerInSeconds -= Time.deltaTime;
		minutes = (int)(timerInSeconds / 60f);
		secondsInMinute = (int)(timerInSeconds % 60f);

		//for testing
		area1 += 1.0f;
		area2 += 2.0f;



		//find player spheres
		if (GameObject.FindGameObjectWithTag ("Player1") && GameObject.FindGameObjectWithTag ("Player2") && !set) {
			SetMaterial1();
			SetMaterial2();
			set = true;
		}



	}

	public float normalizeArea(float a){
		float aMin = 0.0f;
		float aMax = 100 * 100;

		float normalized = (a - aMin) / (aMax - aMin);

		return normalized;

	}

	void SetMaterial1(){
		Debug.Log ("setting material 1");

		GameObject p1 = GameObject.FindGameObjectWithTag ("Player1");
		Renderer renderer = p1.GetComponent<Renderer> ();
		TrailRenderer trenderer = p1.GetComponent<TrailRenderer> ();

		switch (color1) {
		case 0:
			renderer.material = blueMat;
			trenderer.material = blueTrailMat;
			break;
		case 1:
			renderer.material = redMat;
			trenderer.material = redTrailMat;
			break;
		case 2:
			renderer.material = greenMat;
			trenderer.material = greenTrailMat;
			break;
		case 3:
			renderer.material = yellowMat;
			trenderer.material = yellowTrailMat;
			break;
		default:
			Debug.Log ("wrong material!");
			break;
		}

	}

	void SetMaterial2(){
		Debug.Log ("setting material 2");
		
		GameObject p2 = GameObject.FindGameObjectWithTag ("Player2");
		Renderer renderer = p2.GetComponent<Renderer> ();
		TrailRenderer trenderer = p2.GetComponent<TrailRenderer> ();
		
		switch (color2) {
		case 0:
			renderer.material = blueMat;
			trenderer.material = blueTrailMat;
			break;
		case 1:
			renderer.material = redMat;
			trenderer.material = redTrailMat;
			break;
		case 2:
			renderer.material = greenMat;
			trenderer.material = greenTrailMat;
			break;
		case 3:
			renderer.material = yellowMat;
			trenderer.material = yellowTrailMat;
			break;
		default:
			Debug.Log ("wrong material!");
			break;
		}
		
	}


}
