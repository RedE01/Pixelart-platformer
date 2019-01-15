using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{

	[SerializeField]
	private float timer;

	void Start() {
		Destroy(this.gameObject, timer);
	}
}
