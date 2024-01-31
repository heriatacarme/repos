using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public float obstacleSpeed;

	void Start()
    {
		obstacleSpeed = Random.Range(5.0f, 9.0f);
	}

	private void FixedUpdate()
	{
		Vector2 pos = transform.position;

		pos.x -= obstacleSpeed * Time.fixedDeltaTime;
		if (pos.x < -20) Destroy(gameObject);

		transform.position = pos;
	}
}
