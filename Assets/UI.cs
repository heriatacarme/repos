using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    Player player;
    Text distanceText;

    GameObject gameOver;

	private void Awake()
	{
        player = GameObject.Find("Player").GetComponent<Player>();
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();

		gameOver = GameObject.Find("GameOver");
		gameOver.SetActive(false);
	}

    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance.ToString();

		if (player.isDead) gameOver.SetActive(true);
	}

	public void Continue()
	{
		if (player.score >= 5)
		{
			gameOver.SetActive(false);
			player.isDead = false;
			player.score -= 5;
			player.ñoinCounter.text = player.score.ToString();
			player.StartInvincibility();
		}
	}

	public void SaveDistance()
	{
		int distance = Mathf.FloorToInt(player.distance); 
		int numberOfEntries = PlayerPrefs.GetInt("NumberOfEntries", 0);
		PlayerPrefs.SetInt("Distance" + numberOfEntries, distance);
		PlayerPrefs.SetInt("NumberOfEntries", numberOfEntries + 1);
		PlayerPrefs.Save();
	}
}