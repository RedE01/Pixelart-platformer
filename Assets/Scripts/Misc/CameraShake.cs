using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public CinemachineCameraOffset cameraOffset;
	private bool isShaking = false;

	public IEnumerator Shake(float duration, float intensity, float roughness) {
		float shakeTimer = 0;

		Vector2 noisePosition = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

		while (shakeTimer < duration) {
			Vector3 offset = Vector3.zero;
			offset.x = Mathf.PerlinNoise(noisePosition.x, noisePosition.x) - 0.5f;
			offset.y = Mathf.PerlinNoise(noisePosition.y, noisePosition.y) - 0.5f;

			cameraOffset.m_Offset = offset * intensity;

			shakeTimer += Time.deltaTime;
			noisePosition.x += Time.deltaTime * roughness;
			noisePosition.y += Time.deltaTime * roughness;
			yield return null;
		}
		cameraOffset.m_Offset = Vector3.zero;
	}
}
