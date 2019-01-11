using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	public int attackDamage;
	public float attackRange;

	int facingDir;
	Vector2 playerSize;
	LayerMask targetMask;

	void Start() {
		playerSize = GetComponent<CapsuleCollider2D>().size;
		targetMask = LayerMask.GetMask("Enemy");
	}

	void Update() {
		int inputDir = (int)Input.GetAxisRaw("Horizontal");
		if (inputDir != 0) facingDir = inputDir;
		if(Input.GetKeyDown(KeyCode.X)) {
			Collider2D[] targets = Physics2D.OverlapAreaAll(new Vector2(transform.position.x, transform.position.y + playerSize.y * 0.5f), new Vector2(transform.position.x + attackRange * facingDir, transform.position.y - playerSize.y * 0.5f), targetMask);
			foreach(Collider2D t in targets) {
				t.GetComponent<Enemy>().TakeDamage(attackDamage, facingDir);
			}
		}
	}

}
