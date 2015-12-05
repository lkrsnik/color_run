using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{

	// Private references to the objects we will need 
	private Animator animator;
	private CanvasGroup canvasGroup;
	// Property (it is a facade to an Animator's parameter 
	public bool IsOpen {
		get { return animator.GetBool ("IsOpen"); }
		set { animator.SetBool ("IsOpen", value); } 
	}
// Setting the private references and moving the Menu to the 
	// center of the window
	public void Awake ()
	{
		animator = GetComponent<Animator> (); 
		canvasGroup = GetComponent<CanvasGroup> ();
		var rec = GetComponent<RectTransform> ();
		rec.offsetMin = rec.offsetMax = new Vector2 (0, 0);
	}
	// If the menu is active (it is in the "Open" state), make it 
	// interactable
	public void Update ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Open")) { 
			canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
		} else {
			canvasGroup.blocksRaycasts = canvasGroup.interactable = false;
		} 
	}
}
