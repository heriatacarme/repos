using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	Player player;
	[SerializeField] private GameObject[] obstaclePrefabs;
    public float obstacleSpawnTime = 3f;
    private float timeUntilObstacleSpawn;

	private void Awake()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	private void Update()
	{
		SpawnLoop();
	}

	private void SpawnLoop()
	{
		timeUntilObstacleSpawn += Time.deltaTime;

		if (timeUntilObstacleSpawn >= obstacleSpawnTime && !player.isDead)
		{
			Spawn();
			timeUntilObstacleSpawn = 0f;
		}
	}

	private void Spawn()
	{
		GameObject obstaclesToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

		GameObject spawnedObstacle = Instantiate(obstaclesToSpawn, transform.position, Quaternion.identity);

		Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
		obstacleRB.velocity = Vector2.zero;
	}
}
