using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public GameObject target;
	public float damping = 1;
	public Vector3 offset;
	
//	void Start() {
//		offset = target.transform.position - transform.position;
//		offset.x = 0;
//		offset.y = -8;
//		offset.z = 0;
//	}
	
	void LateUpdate() {
		float currentAngle = transform.eulerAngles.y;
		float desiredAngle = target.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);
		
		Quaternion rotation = Quaternion.Euler(0, angle, 0);

		rotation.x = rotation.y = rotation.z = 0;
		transform.position = target.transform.position - (rotation * offset);
		
		transform.LookAt(target.transform);
	}
}