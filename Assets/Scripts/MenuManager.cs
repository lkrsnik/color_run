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
	

}