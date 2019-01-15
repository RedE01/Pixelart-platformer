using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float health;
	public float walkingSpeed;
	public float chasingSpeed;
	public float dazedTime;
	public float chasingRadius;
	public float attackRadius;
	public GameObject damageParticles;
	public GameObject deathParticles;
	public GameObject debris;

	protected enum EnemyState {
		Idle, Chasing, Attacking, Dazed, Death
	};
	protected EnemyState state = EnemyState.Idle;
	protected Vector2 movement;
	protected Vector2 otherMovement;
	protected float dazedTimer;
	protected GameObject player;
	protected Rigidbody2D rb2d;
	protected LayerMask playerLayerMask;
	protected int walkDir = 1;

	private float idleTimeSinceSwitch = 0;
	private float idleSwitchTime = 3.0f;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		rb2d = GetComponent<Rigidbody2D>();
		playerLayerMask = LayerMask.GetMask("Player");
	}

	public void Update() {
		switch(state) {
			case EnemyState.Idle:
				OnIdle();
				if (Physics2D.OverlapCircle(transform.position, chasingRadius, playerLayerMask)) SwitchState(EnemyState.Chasing);
				break;
			case EnemyState.Chasing:
				OnChasing();
				if (!Physics2D.OverlapCircle(transform.position, chasingRadius, playerLayerMask)) SwitchState(EnemyState.Idle);
				if (Physics2D.OverlapCircle(transform.position, attackRadius, playerLayerMask)) SwitchState(EnemyState.Attacking);
				break;
			case EnemyState.Attacking:
				OnAttacking(); //Only switch back state when attack is finished
				break;
			case EnemyState.Dazed:
				OnDazed();
				dazedTimer -= Time.deltaTime;
				if (dazedTimer < 0.0f) SwitchState(EnemyState.Idle);
				break;
			case EnemyState.Death:
				break;
		}
	}

	private void SwitchState(EnemyState newState) {
		state = newState;
		switch(state) {
			case EnemyState.Idle:
				OnIdleEnabled();
				break;
			case EnemyState.Chasing:
				OnChasingEnabled();
				break;
			case EnemyState.Attacking:
				OnAttackingEnabled();
				break;
			case EnemyState.Dazed:
				OnDazedEnabled();
				break;
		}
	}

	public void TakeDamage(int damage, int kbDir) {
		health -= damage;
		dazedTimer = dazedTime;
		state = EnemyState.Dazed;
		rb2d.AddForce(new Vector2(kbDir, 0.5f) * damage * 75);
		Instantiate(damageParticles, transform.position, Quaternion.identity);
		if(health < 0.0f) {
			state = EnemyState.Death;
			Die(kbDir);
		}
	}

	protected virtual void OnIdle() {
		movement.x = walkingSpeed * walkDir;
		idleTimeSinceSwitch += Time.deltaTime;
		if(idleTimeSinceSwitch > idleSwitchTime) {
			idleTimeSinceSwitch = 0.0f;
			walkDir = Mathf.RoundToInt(Random.Range(-1.0f, 1.0f));
		}
	}

	protected virtual void OnChasing() {
		walkDir = (int)Mathf.Sign(player.transform.position.x - transform.position.x);
		movement.x = chasingSpeed * walkDir;
	}
	protected virtual void OnAttacking() {
		movement = Vector2.zero;
		if (!Physics2D.OverlapCircle(transform.position, attackRadius, playerLayerMask)) SwitchState(EnemyState.Chasing);
	}
	protected virtual void OnDazed() {}

	protected virtual void Die(int hitDirection) {
		Instantiate(deathParticles, transform.position, Quaternion.Euler(0, 0, hitDirection < 0 ? 180 : 0));
		Instantiate(debris, transform.position, Quaternion.Euler(0, 0, 90));
		Destroy(this.gameObject);
	}

	protected virtual void OnIdleEnabled() { }
	protected virtual void OnChasingEnabled() { }
	protected virtual void OnAttackingEnabled() { }
	protected virtual void OnDazedEnabled() { }

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, chasingRadius);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRadius);
	}
}
