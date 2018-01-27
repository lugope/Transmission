using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Ignore colisions
	void OnCollisionEnter2D(Collision2D collision){
		if(collision.gameObject.tag != "Ground"){
			Debug.Log("Player ignored by corpse.");
			Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
		}
	}
}
