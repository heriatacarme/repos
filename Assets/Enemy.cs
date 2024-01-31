using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float minY = -1.65f;
	public float maxY = -0.5f;
	private float speed = 1.0f;
	private bool isMovingDown = true;

	void Update()
	{
		if (isMovingDown && transform.position.y > minY) transform.Translate(Vector3.down * speed * Time.deltaTime);
		else {
			isMovingDown = false;
			if (transform.position.y < maxY) transform.Translate(Vector3.up * speed * Time.deltaTime);
			else {
				transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
				isMovingDown = true;
			}
		}
	}
}
