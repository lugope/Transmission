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
			gameObject.GetComponentInChildren<TextMesh> ().text =  ((int)timeLeft).ToString();
		}
	}

	void die(){
		Debug.Log (gameObject.name + " died!");

		Movement script = gameObject.GetComponent<Movement> ();
		script.die();
		deathExplosion ();
	}

	void deathExplosion(){

		RaycastHit2D[] castStar = Physics2D.CircleCastAll (transform.position, explosionRadius, Vector2.zero);
		foreach (RaycastHit2D raycastHit in castStar) {

			GameObject obj = raycastHit.collider.gameObject;


			if(obj.tag == "Player" ){
				Movement script = obj.GetComponent<Movement>();

				script.enabled = true;
				script.initiatePlayer();

			}
			Debug.Log(obj.name + "explosion");
		}
	}
}
