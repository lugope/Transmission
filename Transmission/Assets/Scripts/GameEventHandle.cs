using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameEventHandle : MonoBehaviour {

	//Static singleton property
	public static GameEventHandle Instance {
		get; private set;
	}

	public GameObject initialPlayer;


	void Awake () {
		Instance = this;
	}

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

	public void GameOver (){
		Debug.Log("No one loves me... Game Over!" );
		SceneManager.LoadScene("scene_extreme");
	}

	public void GameWon (){
		SceneManager.LoadScene("scene_extreme");
	}

	void StartGame(){

		if ( initialPlayer ) {

			Movement script = initialPlayer.GetComponent<Movement>();
			script.enabled = true;
			script.initiatePlayer();
		}

	}
}
