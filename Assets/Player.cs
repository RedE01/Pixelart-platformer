using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float walkingSpeed;
	public float jumpVelocity;
	public float groundedTimerMax;
	public float jumpedTimerMax;
	public int dashesMax;
	public float dashDuration;
	public float dashWarmupDuration;
	public float dashSpeed;
	public float climbSpeed;
	public float climbTimerMax;

	private float speed;
	private bool isGrounded;
	private float groundedTimer;
	private float jumpedTimer;
	private float movement;
	private int dashesLeft;
	private bool dashing;
	private float dashTimer;
	private bool warmingUpDash;
	private bool climbing;
	private int facingDir;
	private float climbTimer;
	private Color Dashes0, Dashes1, Dashes2;
	private Vector2 dashDirection;
	private Vector2 colliderSize;
	private float groundAngle;
	private int groundDirection;
	private Vector2 climbDirection;

	private Rigidbody2D rb2d;
	private LayerMask groundLayerMask;
	private SpriteRenderer spr;
	private Collider2D groundCollider;
	private Collider2D climbCollider;
	private PhysicsMaterial2D physMat2d;

	void Start() {
		rb2d = GetComponent<Rigidbody2D>();
		groundLayerMask = LayerMask.GetMask("Ground");
		spr = GetComponent<SpriteRenderer>();
		physMat2d = rb2d.sharedMaterial;

		Dashes0 = new Color(0.0f, 0.0f, 0.0f);
		Dashes1 = new Color(0.5f, 0.5f, 0.5f);
		Dashes2 = new Color(1, 1, 1);

		speed = walkingSpeed;
		groundedTimer = groundedTimerMax;
		jumpedTimer = jumpedTimerMax;
		dashTimer = dashDuration;
		dashesLeft = dashesMax;
		colliderSize = GetComponent<CapsuleCollider2D>().size;
		colliderSize.x = (Mathf.Floor(colliderSize.x * 4) + 1) / 4;
		colliderSize.y = (Mathf.Floor(colliderSize.y * 4) + 1) / 4;
		facingDir = 1;
	}

	void Update() {
		if (!dashing) {
			//Movement
			movement = Input.GetAxis("Horizontal") * speed;
			if (movement * groundDirection <= 0.0f) movement *= groundAngle;
			if (movement != 0.0f) facingDir = movement > 0.0f ? 1 : -1;

			//Jumping
			groundedTimer += Time.deltaTime;
			jumpedTimer += Time.deltaTime;
			groundCollider = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - colliderSize.y * 0.5f), new Vector2(colliderSize.x * 0.95f, 0.1f), 0, groundLayerMask);
			if (groundAngle > 0.3f && groundCollider) {
				groundedTimer = 0.0f;
				climbTimer = 0.0f;
				isGrounded = true;
				dashesLeft = dashesMax;
				spr.color = Dashes2;
				//b2d.gravityScale = 0.0f;
			}
			else {
				isGrounded = false;
			}

			if (Input.GetKeyDown(KeyCode.C)) {
				jumpedTimer = 0.0f;
			}
			if(jumpedTimer < jumpedTimerMax && groundedTimer < groundedTimerMax) {
				rb2d.velocity = new Vector2(movement, jumpVelocity);
				jumpedTimer = jumpedTimerMax;
			}
			if(rb2d.velocity.y > 0.0f && Input.GetKeyUp(KeyCode.C)) {
				rb2d.velocity = new Vector2(movement, rb2d.velocity.y * 0.6f);
			}

			//Climbing
			//if(Input.GetKey(KeyCode.Z)) {
			//	if (climbCollider = Physics2D.OverlapBox(new Vector2(transform.position.x + colliderSize.x * 0.5f * facingDir, transform.position.y), new Vector2(0.2f, colliderSize.y * 0.8f), 0, groundLayerMask)) {
			//		Debug.Log("HELLO");
			//		Climb();
			//	}
			//	else if(Physics2D.OverlapBox(new Vector2(transform.position.x + colliderSize.x * 0.5f * facingDir, transform.position.y - colliderSize.y * 0.5f), new Vector2(0.2f, 0.2f), 0, groundLayerMask) && climbing) {
			//		movement += 1.0f * facingDir;
			//	}
			//	else {
			//		EndClimb();
			//	}
			//}
			//else {
			//	EndClimb();
			//}
		}

		//Dash
		if(Input.GetKeyDown(KeyCode.X) && !dashing && dashesLeft > 0) {
			InitDash();
		}

		if(dashing) {
			dashTimer += Time.deltaTime;
			if(dashTimer > dashWarmupDuration && warmingUpDash) {
				dashTimer = 0.0f;
				warmingUpDash = false;
				BeginDash();
			} 
			if(dashTimer > dashDuration && !warmingUpDash) {
				EndDash();
			}
		}
	}

	void FixedUpdate() {
		if(!dashing) rb2d.velocity = new Vector2(movement, rb2d.velocity.y);
	}

	void OnCollisionStay2D(Collision2D collision) {
		if(collision.collider.CompareTag("Walkable")) {
			if (dashing && !warmingUpDash) {
				if (dashDirection.x * collision.contacts[0].normal.x < 0.0f || dashDirection.y * collision.contacts[0].normal.y < 0.0f) { // A * B < 0.0f checks if they have different signs
					EndDash();
				}
			}
			else if(collision.collider == groundCollider) {
				groundAngle = Mathf.Pow(collision.contacts[0].normal.normalized.y, 2);
				groundDirection = (int)Mathf.Sign(collision.contacts[0].normal.x);
				rb2d.gravityScale = 0.0f;
			}
			//if(collision.collider == climbCollider && climbing) {
			//	Vector2 temp = collision.contacts[0].normal.normalized;
			//	climbDirection = new Vector2(temp.y * 1.5f, Mathf.Abs(temp.x));
			//}
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		rb2d.gravityScale = 1.0f;
	}

	void InitDash() {
		rb2d.velocity = Vector2.zero;
		rb2d.gravityScale = 0.0f;
		dashing = true;
		dashTimer = 0.0f;
		warmingUpDash = true;
		dashesLeft--;
	}

	void BeginDash() {
		dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
		if (dashDirection == Vector2.zero) dashDirection = Vector2.right * facingDir;
		rb2d.velocity = dashDirection * dashSpeed;
		facingDir = dashDirection.x > 0.0f ? 1 : -1;
		Color setColor = Dashes0;
		switch(dashesLeft) {
			case 0: setColor = Dashes0; break;
			case 1: setColor = Dashes1; break;
			case 2: setColor = Dashes2; break;
		}
		spr.color = setColor;
	}

	void EndDash() {
		rb2d.velocity *= 0.25f;
		rb2d.gravityScale = 1.0f;
		dashing = false;
	}

	//void Climb() {
	//	if (climbTimer > climbTimerMax) {
	//		EndClimb();
	//		return;
	//	}
	//	else {
	//		climbing = true;
	//		rb2d.gravityScale = 0.0f;
	//		climbTimer += Time.deltaTime;

	//		rb2d.velocity = climbDirection * climbSpeed * Input.GetAxis("Vertical");
	//	}

	//}

	//void EndClimb() {
	//	rb2d.gravityScale = 1.0f;
	//	//rb2d.velocity = Vector2.zero;
	//	climbing = false;
	//}
}
