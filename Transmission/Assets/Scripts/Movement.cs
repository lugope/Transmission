using UnityEngine;
using System.Collections;


public class Movement : MonoBehaviour {

	public float jumpForce = 6f;
	public float movementForce = 3f;
	public LayerMask groundLayer;
	public bool isDead = false;

	private Timer timer;
	private Rigidbody2D rigidBody;


	void Awake(){
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	void Start () {
		initiatePlayer ();
	}

	public void initiatePlayer(){
		timer = GetComponent<Timer> ();
		gameObject.layer = 0;
		rigidBody.isKinematic = false;
		timer.enabled = true;
	}

	public void die(){
		Stop ();

		gameObject.layer = 0;

		enabled = false;
		isDead = true;


		rigidBody.isKinematic = true;

		GetComponent<BoxCollider2D> ().enabled = false;
		gameObject.GetComponentInChildren<TextMesh> ().text = "";
	}
	

	// Update is called once per frame
	void Update () {

		// if is dead do not run update
		if(isDead){
			Debug.Log ("dead");
			return;
		}


		// Input handle
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			Move(Vector2.right * -1);
		} else if ( Input.GetKeyDown (KeyCode.RightArrow) ){
			Move(Vector2.right);
		}

		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			Stop();
			//Move(Vector2.right);
		} else if ( Input.GetKeyUp (KeyCode.RightArrow) ){
			Stop();
			//Move(Vector2.right * -1);
		}

		if (Input.GetKeyDown (KeyCode.UpArrow) ){
			Jump();
		}

	}


	// Moviment Handle
	void Jump(){
		if(IsGrounded()) {
			rigidBody.AddForce (Vector2.up * jumpForce, ForceMode2D.Impulse );
		}
	}

	public void Stop(){
		rigidBody.velocity = new Vector2 (0f,rigidBody.velocity.y);
	}

	void Move(Vector2 direction){

		rigidBody.AddForce (direction * movementForce, ForceMode2D.Impulse);
	}

	bool IsGrounded() {
		if (Physics2D.Raycast(this.transform.position, Vector2.up * -1, 0.2f, groundLayer.value)) {
			return true;
		}
		else {
			return false;
		} 
	}
}
