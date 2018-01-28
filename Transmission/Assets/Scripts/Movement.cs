using UnityEngine;
using System.Collections;


public class Movement : MonoBehaviour {

	public float jumpForce = 6f;
	public float wallJumpForce = 10f;
	public float movementForce = 3f;
	public LayerMask groundLayer;
	public LayerMask wallLayer;
	public bool isDead = false;
	public bool isFinalTarget = false;
	public GameObject cam;
	public GameObject prefabCorpse; //Reference Corpse to leave behind after death

	private Timer timer;
	private Rigidbody2D rigidBody;
	private SpriteRenderer spriteRenderer;
	private Animator animator;

	bool leftArrowDown = false;
	bool rightArrowDown = false;

	void Awake(){
		rigidBody = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		rigidBody.isKinematic = true;

		animator = GetComponent<Animator> ();
	}

	void Start () {
		initiatePlayer ();
	}

	public void initiatePlayer(){
		timer = GetComponent<Timer> ();
		timer.enabled = true;
		gameObject.layer = 0;
		rigidBody.isKinematic = false;

	}

	public void die(){
		if (!isDead){
			Stop ();

			//GetComponent<BoxCollider2D> ().enabled = false;
			gameObject.GetComponentInChildren<TextMesh> ().text = "";
			gameObject.layer = 0;

			enabled = false;
			isDead = true;

//			cam.GetComponent<CameraController>().follow = null; 
//			cam = null;

			Instantiate(prefabCorpse, gameObject.transform.position, gameObject.transform.rotation);

			Destroy(gameObject);

			Debug.Log(gameObject.name + " Dead!");
		}
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

		//Chack status of animation
		CheckAnimationState();
	}

    //	Animation Handler
	void CheckAnimationState(){
		
		//Jump animation
		if ( IsGrounded() ) {
			animator.SetBool("isGrounded", true);
			animator.SetBool("isRunning", false);

		} else {
			animator.SetBool("isGrounded", false);
		}

		//Running animation
		if ( Mathf.Abs(rigidBody.velocity.x) > 0 ) {
			animator.SetBool("isRunning", true);

		} else {
			animator.SetBool("isRunning", false);
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

			if ( Physics2D.Raycast(this.transform.position, Vector2.right, 0.2f, groundLayer.value) ) {
				rigidBody.AddForce (Vector2.left * wallJumpForce, ForceMode2D.Impulse );

			} else if (Physics2D.Raycast(this.transform.position, Vector2.left, 0.2f, groundLayer.value)){
				rigidBody.AddForce (Vector2.right * wallJumpForce, ForceMode2D.Impulse );
			}
		}
	}

	public void Stop(){
		
		float vely = rigidBody.velocity.y;
		rigidBody.velocity = new Vector2 (0f,vely);

	}

	void Move(Vector2 direction){
		rigidBody.velocity = new Vector2( (direction * movementForce).x, rigidBody.velocity.y);
//		rigidBody.AddForce (direction * movementForce, ForceMode2D.Impulse);
	}

	bool IsGrounded() {
		bool raycastGround = Physics2D.Raycast(this.transform.position, Vector2.down, 0.2f, groundLayer.value);
		bool rightRaycast = Physics2D.Raycast(this.transform.position, Vector2.right, 0.2f, groundLayer.value);
		bool leftRaycast = Physics2D.Raycast(this.transform.position, Vector2.left, 0.2f, groundLayer.value);

		if (raycastGround|| rightRaycast || leftRaycast) {
			return true;

		} else {
			return false;
		} 
	}

	//Physics Handle

	//Ignore colisions
	void OnCollisionEnter2D(Collision2D collision){
		if(collision.gameObject.tag == "Player"){
			//Debug.Log("Player ignored by corpse.");
			Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
		}
	}


}
