using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject follow;

	private Vector3 offset;

	// Use this for initialization
	void Start () {

		if(follow){
			offset = transform.position - follow.transform.position;
		}
	}

	// Late Update is called after frame update
	void LateUpdate () {

		// If follow exists, update camera position
		if( follow ){
			transform.position = new Vector3 ( transform.position.x, follow.transform.position.y + offset.y, transform.position.z);
		}

	}

	public void updateFollow(GameObject newFollow){
		
		follow = newFollow;
		Start();

//		Debug.Log("Updating camera follow...");
	}
}
