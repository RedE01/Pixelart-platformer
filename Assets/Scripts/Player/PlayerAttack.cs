using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	public int attackDamage;
	public Transform attackPos;
	public float attackRadius;

	Vector2 playerSize;
	LayerMask targetMask;
	Player playerScript;
	Rigidbody2D rb2d;
	CameraShake cameraShake;
	Animator animator;

	void Start() {
		playerSize = GetComponent<CapsuleCollider2D>().size;
		targetMask = LayerMask.GetMask("Enemy");
		playerScript = GetComponent<Player>();
		rb2d = GetComponent<Rigidbody2D>();
		cameraShake = Camera.main.GetComponent<CameraShake>();
		animator = GetComponent<Animator>();
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.X)) {
			attackPos.localPosition = new Vector2(Mathf.Abs(attackPos.localPosition.x) * playerScript.facingDir, attackPos.localPosition.y);
			Collider2D[] targets = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, targetMask);

			animator.SetTrigger("Attack");

			if(targets.Length > 0) {
				Collider2D closest = targets[0];
				float closestDistance = Vector2.Distance(closest.transform.position, this.transform.position);

				for(int i = 1; i < targets.Length; i++) {
					float dist = Vector2.Distance(targets[i].transform.position, this.transform.position);

					if(dist < closestDistance) {
						closestDistance = dist;
						closest = targets[i];
					}
				}

				if(closest.GetComponent<Enemy>().TakeDamage(attackDamage, playerScript.facingDir)) {
					StartCoroutine(cameraShake.Shake(0.2f, 0.75f, 4.0f));
				}
				StartCoroutine(cameraShake.Shake(0.1f, 0.3f, 3.0f));
			}
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPos.position, attackRadius);
	}

}
