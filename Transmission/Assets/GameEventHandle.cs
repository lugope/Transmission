using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventHandle : MonoBehaviour {

	public GameObject initialPlayer;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		checkPressAnyKey();
	}

	void checkPressAnyKey(){

		if ( Input.anyKeyDown ) {
			StartGame();
		}

	}

	void StartGame(){

		if ( initialPlayer ) {

			Movement script = initialPlayer.GetComponent<Movement>();
			script.enabled = true;
			script.initiatePlayer();
		}

	}
}
