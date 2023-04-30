using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI scoreText;

	[SerializeField]
	private GameObject parcelPrefab;

	[SerializeField]
	private List<DeliveryRoute> deliveryRoutes = new List<DeliveryRoute>();

	private int score = 0;
	private static GameManager instance;

	[Serializable]
	public class DeliveryRoute
	{
		public Indicator indicator;
		public GameObject destination;
		public Transform parcelSpawnPoint;
	}

	void Start()
	{
		instance = this;
		foreach (DeliveryRoute deliveryRoute in deliveryRoutes)
		{
			Instantiate(parcelPrefab, deliveryRoute.parcelSpawnPoint.position, Quaternion.identity);
		}
	}

	public static void AddScore(int score)
	{
		instance.score += score;
		instance.scoreText.text = instance.score.ToString();
	}
}