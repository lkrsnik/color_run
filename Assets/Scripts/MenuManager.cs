using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	// Currently open menu. Set the initial reference in the Inspector 
	public Menu currentMenu;

	void Start ()
	{
		// For showing the main menu
		ShowMenu (currentMenu);  
		//setting up GameManager
		DontDestroyOnLoad(GameManager.Instance);
	}
	// Activate the given menu and deactivate the current 
	// Invokable by the Inspector
	public void ShowMenu (Menu menu)
	{
		if (currentMenu != null) 
			currentMenu.IsOpen = false;
		currentMenu = menu;
		currentMenu.IsOpen = true; 
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
		Application.LoadLevel (level);

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
		GameManager.instance.color1 = 0;
		GameManager.instance.cColor1 = Color.blue;
		Debug.Log ("Blue Selected");
	}
	public void ColorSelected1Red(){
		GameManager.instance.color1 = 1;
		GameManager.instance.cColor1 = Color.red;
		Debug.Log ("Red Selected");
	}
	public void ColorSelected1Green(){
		GameManager.instance.color1 = 2;
		GameManager.instance.cColor1 = Color.green;
		Debug.Log ("Green Selected");
	}
	public void ColorSelected1Yellow(){
		GameManager.instance.color1 = 3;
		GameManager.instance.cColor1 = Color.yellow;
		Debug.Log ("Yellow Selected");
	}

	public void ColorSelected2Blue(){
		GameManager.instance.color2 = 0;
		GameManager.instance.cColor2 = Color.blue;
		Debug.Log ("Blue Selected");
	}
	public void ColorSelected2Red(){
		GameManager.instance.color2 = 1;
		GameManager.instance.cColor2 = Color.red;
		Debug.Log ("Red Selected");
	}
	public void ColorSelected2Green(){
		GameManager.instance.color2 = 2;
		GameManager.instance.cColor2 = Color.green;
		Debug.Log ("Green Selected");
	}
	public void ColorSelected2Yellow(){
		GameManager.instance.color2 = 3;
		GameManager.instance.cColor2 = Color.yellow;
		Debug.Log ("Yellow Selected");
	}



}