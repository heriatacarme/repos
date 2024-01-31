using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
	private Transform entryContainer;
	private Transform template;
	private List<HighscoreEntry> highscoreEntryList;
	private List<Transform> highscoreEntryTransformList;

	private void Awake()
	{
		entryContainer = GameObject.Find("Container").transform;
		template = GameObject.Find("Table").transform;

		template.gameObject.SetActive(false);

		highscoreEntryList = new List<HighscoreEntry>();
		int numberOfEntries = PlayerPrefs.GetInt("NumberOfEntries", 0);
		for (int i = 0; i < numberOfEntries; i++)
		{
			int savedScore = PlayerPrefs.GetInt("Distance" + i);
			highscoreEntryList.Add(new HighscoreEntry { score = savedScore });
		}

		for (int i = 0; i < highscoreEntryList.Count; i++)
		{
			for (int j = i + 1; j < highscoreEntryList.Count; j++)
			{
				if (highscoreEntryList[j].score > highscoreEntryList[i].score)
				{
					HighscoreEntry temp = highscoreEntryList[i];
					highscoreEntryList[i] = highscoreEntryList[j];
					highscoreEntryList[j] = temp;
				}
			}
		}

		highscoreEntryTransformList = new List<Transform>();
		foreach(HighscoreEntry highscoreEntry in highscoreEntryList)
		{
			CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
		}
	}

	private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
	{
		if (transformList.Count < 3)
		{
			float tableHeight = 50f;
			Transform transform = Instantiate(template, container);
			RectTransform rectTransform = transform.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, tableHeight - tableHeight * transformList.Count);
			transform.gameObject.SetActive(true);

			int rank = transformList.Count + 1;
			int score = highscoreEntry.score;
			transform.Find("Position").GetComponent<Text>().text = rank.ToString();
			transform.Find("Score").GetComponent<Text>().text = score.ToString();

			transformList.Add(transform);
		}
	}

	private class HighscoreEntry
	{
		public int score;
	}
}
