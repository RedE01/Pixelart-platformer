using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

	public IEnumerator setTimeScaleForSeconds(float timeScale, float seconds) {
		Time.timeScale = timeScale;

		yield return new WaitForSecondsRealtime(seconds);

		Time.timeScale = 1.0f;
	}

}
