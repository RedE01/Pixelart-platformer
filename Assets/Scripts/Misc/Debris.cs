using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{

	private LayerMask layerMask;

	void Start() {
		layerMask = LayerMask.GetMask("Ground");
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (layerMask.value >> collision.gameObject.layer == 1) {
			Rigidbody2D rb = GetComponent<Rigidbody2D>();
			if (rb) {
				Destroy(rb);
				Destroy(GetComponent<BoxCollider2D>());
			}
			Destroy(this);
		}
	}
}
