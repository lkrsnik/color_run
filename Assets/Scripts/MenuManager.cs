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

	public Camera mainCamera;
	public Camera menuCamera;

	private bool onPause;
	bool set = false;


	void Start ()
	{
		// For showing the main menu
		ShowMenu (currentMenu);  

		setToMenuCamera ();


		onPause = false;

		//setting up GameManager
		DontDestroyOnLoad(GameManager.Instance);
		if (!set) {
			//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			set = true;
		}

		//DontDestroyOnLoad (mainMenu);
	}

	void Update(){
		if (Input.GetKeyUp (KeyCode.Escape) && !onPause && (currentMenu == hudMenu)) {
//		if (Input.GetKeyUp (KeyCode.Escape) && !onPause) {
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


//	void OnLevelWasLoaded(int level) {
//		if (level == 0)
//			print("Loaded Level " + level);
//		//not working yet (menu not showing up...)
//			Debug.Log (mainMenu);
//		    currentMenu = mainMenu;
//			Start ();
////			currentMenu = mainMenu;
////			currentMenu.IsOpen = true; 
////			ShowMenu (mainMenu);
//		
//	}


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
//		Application.LoadLevel (level);
		SceneManager.LoadScene(level);

		// reset level properties 
		GameManager.instance.ResetLevel ();

	}

	public void GetInput1(string playerName1){
		GameManager.instance.name1 = playerName1;
		Debug.Log ("Name player 1: " + playerName1);
	}

	public void GetInput2(string playerName2){
		GameManager.instance.name2 = playerName2;
		Debug.Log ("Name player 2: " + playerName2);
	}

	public void ColorSelected1Blue(){
		GameManager.instance.color1 = 3;
		GameManager.instance.cColor1 = Color.blue;
		Debug.Log ("Blue Selected");
	}
	public void ColorSelected1Red(){
		GameManager.instance.color1 = 5;
		GameManager.instance.cColor1 = Color.red;
		Debug.Log ("Red Selected");
	}
	public void ColorSelected1Green(){
		GameManager.instance.color1 = 2;
		GameManager.instance.cColor1 = Color.green;
		Debug.Log ("Green Selected");
	}
	public void ColorSelected1Yellow(){
		GameManager.instance.color1 = 4;
		GameManager.instance.cColor1 = Color.yellow;
		Debug.Log ("Yellow Selected");
	}

	public void ColorSelected2Blue(){
		GameManager.instance.color2 = 3;
		GameManager.instance.cColor2 = Color.blue;
		Debug.Log ("Blue Selected");
	}
	public void ColorSelected2Red(){
		GameManager.instance.color2 = 5;
		GameManager.instance.cColor2 = Color.red;
		Debug.Log ("Red Selected");
	}
	public void ColorSelected2Green(){
		GameManager.instance.color2 = 2;
		GameManager.instance.cColor2 = Color.green;
		Debug.Log ("Green Selected");
	}
	public void ColorSelected2Yellow(){
		GameManager.instance.color2 = 4;
		GameManager.instance.cColor2 = Color.yellow;
		Debug.Log ("Yellow Selected");
	}

	public void StartGame(){
		Debug.Log ("Started the game");
		//Application.LoadLevel (0);
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		GameManager.instance.ResetLevel ();
		ShowMenu (hudMenu);
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

	public void RestartLevel(){
		//Application.LoadLevel (0);
		GameManager.instance.ResetLevel ();
	}


}