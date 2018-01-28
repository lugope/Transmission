using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float countDown = 5f;
	public float explosionRadius = 5f;
	private TextMesh countDownText;
	private float timeLeft = 0f;
	private bool done = false;



	void Start () {
		timeLeft = countDown;
		countDownText = gameObject.GetComponentInChildren<TextMesh>();
	}

	// Update is called once per frame
	void Update () {

		if(done){
			return;
		}

		timeLeft -= Time.deltaTime;
		if (timeLeft < 0f) {
			die ();
			done = true;
		} else {
			countDownText.text =  ((int)timeLeft).ToString();
		}
	}

	void die(){
		Debug.Log (gameObject.name + " died!");

		Movement script = gameObject.GetComponent<Movement> ();
		deathExplosion ();
		script.die();
	}

	void deathExplosion(){

		bool noOneToLoveMe = true;

		int i = 0;
		RaycastHit2D[] castStar = Physics2D.CircleCastAll (transform.position, explosionRadius, Vector2.zero);
		foreach (RaycastHit2D raycastHit in castStar) {

			GameObject obj = raycastHit.collider.gameObject;



			if(obj.tag == "Player" && obj != gameObject){
				i++;
				Movement script = obj.GetComponent<Movement>();

				if ( script.isFinalTarget ) {
					Debug.Log(" Kabuum!");
					Debug.Log(obj.name + " Infected, Mission Complete!");
					noOneToLoveMe = false;

				} else {
					
//					Debug.Log(gameObject.GetComponent<Movement>().cam);

					//Transmitting the camera
//					script.cam = gameObject.GetComponent<Movement>().cam;
//					CameraController cam = script.cam.GetComponent<CameraController>();
//					cam.updateFollow(script.gameObject);

					//Running next player
					script.enabled = true;
					script.initiatePlayer();
					noOneToLoveMe = false;
				}
			}
		}

		if ( noOneToLoveMe ) {
			GameEventHandle.Instance.GameOver();

		}
	}
}
