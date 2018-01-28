using UnityEngine;
using System.Collections;


public class Movement : MonoBehaviour {

	public float jumpForce = 2.5f;
	public float wallJumpForce = 8f;
	public float movementForce = 1.4f;
	public LayerMask groundLayer;
	public LayerMask wallLayer;
	public bool isDead = false;
	public bool isFinalTarget = false;
	public GameObject cam;
	public GameObject prefabCorpse; //Reference Corpse to leave behind after death
	public GameObject deathParticle; //Particle FX from the death

	private Timer timer;
	private Rigidbody2D rigidBody;
	private SpriteRenderer spriteRenderer;
	private Animator animator;

	private int jumpTimeIterator = 0;
	public int jumpMaxTimeIterator = 7;

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

			//Instantiate death particles
			if(deathParticle){
				Instantiate(deathParticle, gameObject.transform.position, gameObject.transform.rotation);
			}


//			deathTimer t = GetComponent<deathTimer>();
//
//			if(t){
//				t.enabled = true;
//			}

			Destroy(gameObject);

			//Debug.Log(gameObject.name + " Dead!");
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

		if ( jumpTimeIterator > 0 ) {
			jumpTimeIterator -= 1;
		}
	}

    //	Animation Handler
	void CheckAnimationState(){

		int grounded = IsGrounded();

		//Jump animation
		if ( grounded % 2 == 1 ) {
			animator.SetBool("isGrounded", true);
			//GameEventHandle.Instance.playWalk();

		} else if ( grounded % 2 == 0 && grounded != 0 ) {
			animator.SetBool("isGrounded", false);
			animator.SetBool("isWalling", true);

		} else {
			animator.SetBool("isGrounded", false);
			animator.SetBool("isWalling", false);
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
			//Debug.Log ("dead");
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


		if (Input.GetKeyDown (KeyCode.Space) ){
			Jump();
		}
	}

	int grounded = 0;

	// Moviment Handle
	void Jump(){


		if( jumpTimeIterator < jumpMaxTimeIterator/2 && jumpTimeIterator > 0 && grounded == 0 ){
			
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
			jumpWall();
		}

		grounded = IsGrounded();

		//Debug.Log( grounded );

		if ( grounded > 0) {

			GameEventHandle.Instance.playJump();


			rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);

			if(grounded == 1 || grounded == 5 || grounded == 9){
				rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
				grounded = 0;
				return;
			}

			if ( grounded == 4 ) {
				//Same side of the wall
				if( Input.GetKey(KeyCode.RightArrow) ){ 
					rigidBody.AddForce(Vector2.left * jumpForce, ForceMode2D.Impulse);
				}
				rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

			}

			if ( grounded == 8 ) {
				//Same side of the wall
				if( Input.GetKey(KeyCode.LeftArrow) ){ 
					rigidBody.AddForce(Vector2.right * jumpForce, ForceMode2D.Impulse);
				}
				rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			}
		}

		grounded = 0;
		return;
	}


	//Wall jump to oposite side
	public void jumpWall(){

		//x movement
		if ( Input.GetKey(KeyCode.LeftArrow) ) {
			rigidBody.AddForce(Vector2.left * wallJumpForce, ForceMode2D.Impulse);
		}
	
		if ( Input.GetKey(KeyCode.RightArrow) ) {
			rigidBody.AddForce(Vector2.right * wallJumpForce, ForceMode2D.Impulse);
		}

		//Apply jump
		rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

		jumpTimeIterator = 0;
	}

	public void Stop(){
		float vely = rigidBody.velocity.y;
		rigidBody.velocity = new Vector2 (0f,vely);
	}

	void Move(Vector2 direction){
		rigidBody.velocity = new Vector2( (direction * movementForce).x, rigidBody.velocity.y);
//		rigidBody.AddForce (direction * movementForce, ForceMode2D.Impulse);
	}

	int IsGrounded() {
		
		int raycastGround = Physics2D.Raycast(this.transform.position, Vector2.down, 0.16f, groundLayer.value)? 1:0;
		int rightRaycast = Physics2D.Raycast(this.transform.position, Vector2.right, 0.1f, groundLayer.value)? 4:0;
		int leftRaycast = Physics2D.Raycast(this.transform.position, Vector2.left, 0.1f, groundLayer.value)? 8:0;

		int sum = raycastGround + rightRaycast + leftRaycast;
		//Debug.Log(raycastGround + " " + rightRaycast + " " + leftRaycast);

		if( sum%2 == 0 && sum != 0){
			jumpTimeIterator = jumpMaxTimeIterator;
		}

		return sum;
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
