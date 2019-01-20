using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	[HideInInspector]
	public TimeManager timeManager;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this.gameObject);

		timeManager = GetComponent<TimeManager>();
	}
}
