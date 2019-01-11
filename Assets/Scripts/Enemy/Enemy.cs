using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	Rigidbody2D rb2d;

	void Start() {
		rb2d = GetComponent<Rigidbody2D>();
	}

	public void TakeDamage(int damage, int kbDir) {
		rb2d.AddForce(new Vector2(kbDir, 0.5f) * damage * 75);
	}
}
