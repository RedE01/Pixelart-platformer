using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy {
	
	protected override void OnIdle() {
		base.OnIdle();
	}

	protected override void  OnDazed() {
		movement = Vector2.zero;
	}

	void FixedUpdate() {
		rb2d.velocity = new Vector2(movement.x + otherMovement.x, otherMovement.y + rb2d.velocity.y);
	}
}
