using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	// Currently open menu. Set the initial reference in the Inspector 
	public Menu currentMenu;
	public Menu mainMenu;
	public Menu pauseMenu;
	public Menu hudMenu;
	public Menu finishMenu;

	public Camera mainCamera;
	public Camera menuCamera;

	private bool onPause;
	private bool muted;


	void Start ()
	{
		// For showing the main menu
		ShowMenu (currentMenu);  

		setToMenuCamera ();

		onPause = false;

		//setting up GameManager
		DontDestroyOnLoad(GameManager.Instance);

		muted = false;
			
	}

	void Update(){
		if (Input.GetKeyUp (KeyCode.Escape) && !onPause && (currentMenu == hudMenu)) {
			//pause
			PauseGame();

		} else if (Input.GetKeyUp (KeyCode.Escape) && onPause) {
			ResumeGame ();
			ShowMenu (hudMenu);
		}
	}

	void setToMainCamera(){
		mainCamera.enabled = true;
		menuCamera.enabled = false;
	}

	void setToMenuCamera(){
		mainCamera.enabled = false;
		menuCamera.enabled = true;
	}


	// Activate the given menu and deactivate the current 
	// Invokable by the Inspector
	public void ShowMenu (Menu menu)
	{
		if (currentMenu != null) 
			currentMenu.IsOpen = false;
		currentMenu = menu;
		currentMenu.IsOpen = true; 
		if (currentMenu == hudMenu) {
			setToMainCamera ();
		} else {
			setToMenuCamera ();
		}
	}

	public void Quit ()
	{
		Application.Quit (); 
		//UnityEditor.EditorApplication.isPlaying = false;
	}

	public void ShowScene (int level)
	{
		if (currentMenu != null)
			currentMenu.IsOpen = false;
		currentMenu = null;
		SceneManager.LoadScene(level);

		// reset level properties 
		GameManager.instance.ResetLevel ();

	}

	public void GetInput0(string playerName0){
		Debug.Log ("Name player 0: " + playerName0);
		GameManager.instance.players[0].pName = playerName0;
	}

	public void GetInput1(string playerName1){
		Debug.Log ("Name player 1: " + playerName1);
		GameManager.instance.players[1].pName = playerName1;
	}

	public void ColorSelectedGreen(int playerID){
		GameManager.instance.players[playerID].colorID = 2;
		GameManager.instance.players[playerID].color = Color.green;
		Debug.Log ("Green Selected");
	}
	public void ColorSelectedBlue(int playerID){
		GameManager.instance.players[playerID].colorID = 3;
		GameManager.instance.players[playerID].color = Color.blue;
		Debug.Log ("Blue Selected");
	}
	public void ColorSelectedYellow(int playerID){
		GameManager.instance.players[playerID].colorID = 4;
		GameManager.instance.players[playerID].color = Color.yellow;
		Debug.Log ("Yellow Selected");
	}
	public void ColorSelectedRed(int playerID){
		GameManager.instance.players[playerID].colorID = 5;
		GameManager.instance.players[playerID].color = Color.red;
		Debug.Log ("Red Selected");
	}



	public void StartGame(){
		//no material set -> default P0 -> blue, P1 -> red
		if (GameManager.instance.players [0].colorID == 0) {
			GameManager.instance.players [0].colorID = 3;
			GameManager.instance.players [0].color = Color.blue;
		}
		if (GameManager.instance.players [1].colorID == 0) {
			GameManager.instance.players [1].colorID = 5;
			GameManager.instance.players [1].color = Color.red;
		}

		Debug.Log ("Started the game");
//		Debug.Log ("Value Color:" + GameManager.instance.players[0].colorID);
		GameManager.instance.ResetLevel ();
		ShowMenu (hudMenu);
		Time.timeScale = 1;
	}

	public void PauseGame(){
		Debug.Log ("Pressed: ESC -> pause ");
		onPause = true;
		ShowMenu (pauseMenu);
		Time.timeScale = 0;
	}

	public void ResumeGame(){
		Debug.Log ("Pressed: ESC -> UNpause ");
		onPause = false;
		Time.timeScale = 1;
//		ShowMenu (hudMenu);
	}
		
	public void setTime0(){
		Time.timeScale = 0;
	}

	public void RestartLevel(){
		GameManager.instance.ResetLevel ();
	}

	public void FinishedGame(){
		ShowMenu (finishMenu);
		Debug.Log ("Finished Game");
		Debug.Log ("Winner: Player" + GameManager.instance.winnerID);
		Time.timeScale = 0;
		GameManager.instance.gameFinished = true;
	}

	public void Level1(){
		Debug.Log ("Mode1 Selected");
		GameManager.instance.SetControllerMode (0);
	}

	public void Level2(){
		Debug.Log ("Mode2 Selected");
		GameManager.instance.SetControllerMode (1);
	}

	public void GetAudioVolume(float volume){
		GameManager.instance.audioVolume = volume;
	}

	public void MuteUnmuteAudio(){
		if (muted == true) {
			//unmute
			GameManager.instance.audioVolume = 1.0f;
			muted = false;
		} else if (muted == false) {
			//mute
			GameManager.instance.audioVolume = 0.0f;
			muted = true;
		}
	}

	public void SwitchNightEnabled(){
		if (GameManager.instance.dayNightEnabled == true) {
			GameManager.instance.dayNightEnabled = false;
			for(int i=0;i<4;i++){
				GameObject p = GameObject.FindGameObjectWithTag ("Player" + i);
				p.GetComponent<Light> ().enabled = false;
			}
		} else if (GameManager.instance.dayNightEnabled == false) {
			GameManager.instance.dayNightEnabled = true;
			for(int i=0;i<4;i++){
				GameObject p = GameObject.FindGameObjectWithTag ("Player" + i);
				p.GetComponent<Light> ().enabled = true;
			}
		}
	}


}