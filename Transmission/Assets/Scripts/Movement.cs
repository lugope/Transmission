using UnityEngine;
using System.Collections;


public class Movement : MonoBehaviour {

	public float jumpForce = 6f;
	public float movementForce = 3f;
	public LayerMask groundLayer;
	public bool isDead = false;

	private Timer timer;
	private Rigidbody2D rigidBody;
	private SpriteRenderer spriteRenderer;
	private bool XDirection = true; //Gets the direction that the character is facing

	bool leftArrowDown = false;
	bool rightArrowDown = false;

	void Awake(){
		rigidBody = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void Start () {
		initiatePlayer ();
	}

	public void initiatePlayer(){
//		timer = GetComponent<Timer> ();
//		timer.enabled = true;

		gameObject.layer = 0;
		rigidBody.isKinematic = false;

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

		handleInput();

		//Flip image on the X axis
		if(rigidBody.velocity.x > 0){
			spriteRenderer.flipX = false;

		} else if(rigidBody.velocity.x < 0){
			spriteRenderer.flipX = true;
		}

	}
		

	void handleInput(){

		// if is dead do not handle input
		if(isDead){
			Debug.Log ("dead");
			return;
		}

		leftArrowDown = Input.GetKey(KeyCode.LeftArrow);
		rightArrowDown = Input.GetKey(KeyCode.RightArrow);

		// Input handle
		if (leftArrowDown) {
			Move(Vector2.left );
		} else if ( rightArrowDown ){
			Move(Vector2.right);
		}

		//Stop if up
		if ( !leftArrowDown && !rightArrowDown ) {
			Stop();
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
		rigidBody.velocity = new Vector2( (direction * movementForce).x, rigidBody.velocity.y);
//		rigidBody.AddForce (direction * movementForce, ForceMode2D.Impulse);
	}

	bool IsGrounded() {
		if (Physics2D.Raycast(this.transform.position, Vector2.down, 0.2f, groundLayer.value)) {
			return true;
		}
		else {
			return false;
		} 
	}
}
