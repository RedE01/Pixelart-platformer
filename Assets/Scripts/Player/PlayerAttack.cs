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

	void Start() {
		playerSize = GetComponent<CapsuleCollider2D>().size;
		targetMask = LayerMask.GetMask("Enemy");
		playerScript = GetComponent<Player>();
		rb2d = GetComponent<Rigidbody2D>();
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.X)) {
			attackPos.localPosition = new Vector2(Mathf.Abs(attackPos.localPosition.x) * playerScript.facingDir, attackPos.localPosition.y);
			Collider2D[] targets = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, targetMask);
			foreach(Collider2D t in targets) {
				t.GetComponent<Enemy>().TakeDamage(attackDamage, playerScript.facingDir);
			}
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPos.position, attackRadius);
	}

}
