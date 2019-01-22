using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentInteractable : MonoBehaviour
{

	private Animator animator;

	void Start() {
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.CompareTag("Player") || collision.CompareTag("Entity")) {
			animator.SetTrigger("Interact");
		}
	}
}
