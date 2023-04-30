using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI scoreText;

	private int score = 0;
	private static ScoreManager instance;

	void Start()
	{
		instance = this;
	}

	public static void AddScore(int score)
	{
		instance.score += score;
		instance.scoreText.text = instance.score.ToString();
	}
}