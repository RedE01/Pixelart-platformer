using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject Enemy;

    void Start()
    {
		StartCoroutine(spawnEnemy());
    }

	private IEnumerator spawnEnemy() {
		Instantiate(Enemy, transform.position, Quaternion.identity);

		yield return new WaitForSecondsRealtime(0.5f);

		StartCoroutine(spawnEnemy());
	}
}
