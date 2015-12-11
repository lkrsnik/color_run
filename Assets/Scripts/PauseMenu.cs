using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {


	private Animator anim;
	private bool panelDisplayed;
	public GameObject panel;

	void Start(){
		anim = panel.GetComponent<Animator> ();
		anim.enabled = false;
		panelDisplayed = false;
	}

	void Update(){
		if (Input.GetKeyUp (KeyCode.Escape) && !panelDisplayed) {
			//pause
			ShowPanel ();
		} else if (Input.GetKeyUp (KeyCode.Escape) && panelDisplayed) {
			HidePanel();
		}
	}

	private void ShowPanel(){
		anim.enabled = true;
		anim.Play ("PauseSlideIn");
		panelDisplayed = true;
		Time.timeScale = 0;
	}

	private void HidePanel(){
		panelDisplayed = false;
		anim.Play ("PauseSlideOut");
		Time.timeScale = 1;	
	}

	public void ReturnToMain(){
		Application.LoadLevel (0);

	}

	public void RestartLevel(){
		Application.LoadLevel (1);
		GameManager.instance.ResetLevel ();
		HidePanel ();
	}

	public void Resume(){
		HidePanel ();
	}
//	private CanvasGroup canvasGroup;
//	bool isPause = false;
//	
//	void Update () {
//		if( Input.GetKeyDown(KeyCode.Escape))
//		{
//			isPause = !isPause;
//
//
//
////				if (isPause){
////					Time.timeScale = 0;
////				IsOpen = true;
////
////			}
////			    else {
////				Time.timeScale = 1;
////			}
//
//
//			if (isPause) { 
//				//canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
//				Time.timeScale = 0;
//				var rec = GetComponent<RectTransform> ();
//				rec.offsetMin = rec.offsetMax = new Vector2 (0, 0);
//			} else {
//				//canvasGroup.blocksRaycasts = canvasGroup.interactable = false;
//				Time.timeScale = 1;
//
//				var rec = GetComponent<RectTransform> ();
//				rec.offsetMin = rec.offsetMax = new Vector2 (0, 250);
//			} 
//		}
//	}
//
//	public void rstLvl(){
//		GameManager.instance.ResetLevel ();
//	}

//	public void Update ()
//	{
//		if (isPause) { 
//			canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
//			Time.timeScale = 0;
//		} else {
//			canvasGroup.blocksRaycasts = canvasGroup.interactable = false;
//			Time.timeScale = 1;
//		} 
//	}

	// Setting the private references and moving the Menu to the 
	// center of the window
//	public void Awake ()
//	{
//
//		canvasGroup = GetComponent<CanvasGroup> ();
//		var rec = GetComponent<RectTransform> ();
//		rec.offsetMin = rec.offsetMax = new Vector2 (0, 0);
//	}

//	public Rect windowRect = new Rect(20, 20, 120, 50);
//	void OnGUI()
//	{
//		if (isPause) {
//
//		}
//			//windowRect = GUI.Window(0, windowRect, DoMyWindow, "Pause");
//	}
//	
//	void DoMyWindow(int windowID) {
//		if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
//			print("Got a click");
//		
//	}
}
