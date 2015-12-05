using UnityEngine;
using System.Collections;

public class WallCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.contacts[0].otherCollider.gameObject.tag == "Wall")
		    print ("I colided!");

	}
}
