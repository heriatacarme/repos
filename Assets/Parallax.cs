using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	Player player; 
	public float speed = 4f;
	private Vector2 direction = Vector2.left;
	private float resetPosition = 0.0f;
	private Vector3 startPosition;

	private void Awake()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	void Start()
	{
		startPosition = transform.position;
	}

	void Update()
	{
		if (!player.isDead)
		{
			Vector3 offset = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
			transform.position += offset;

			if (transform.position.x <= resetPosition) transform.position = startPosition;
		}
	}
}
