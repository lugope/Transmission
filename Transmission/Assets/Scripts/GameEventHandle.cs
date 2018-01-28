using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameEventHandle : MonoBehaviour {

	public Camera camera;

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
		cameraFollow(initialPlayer);
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
		SceneManager.LoadScene("tile_scene");
	}

	public void GameWon (){
		SceneManager.LoadScene("tile_scene");
	}

	public void cameraFollow(GameObject obj){
		CameraController script = camera.GetComponent<CameraController>(); 
		script.updateFollow(obj);
	}

	void StartGame(){

		if ( initialPlayer ) {

			Movement script = initialPlayer.GetComponent<Movement>();
			script.enabled = true;
			script.initiatePlayer();
		}

	}
}
