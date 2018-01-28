using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathTimer : MonoBehaviour {

	public float countDown = 5f;
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
			
		}
	}

	void die(){

		Destroy(gameObject);

		//gameObject.GetComponent<Movement>().destruction();
	}
}
