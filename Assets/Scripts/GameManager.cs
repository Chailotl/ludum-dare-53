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
	private Player player;

	[SerializeField]
	private List<DeliveryRoute> deliveryRoutes = new List<DeliveryRoute>();

	private int score = 0;
	private static GameManager instance;
	private AudioSource audio;

	public static int parcelsStolen = 0;

	[Serializable]
	public class DeliveryRoute
	{
		public Indicator indicator;
		public GameObject destination;
		public Transform parcelSpawnPoint;
		public int points;
	}

	void Start()
	{
		instance = this;
		audio = GetComponent<AudioSource>();
		foreach (DeliveryRoute route in deliveryRoutes)
		{
			GameObject parcel = Instantiate(parcelPrefab, route.parcelSpawnPoint.position, Quaternion.identity);
			parcel.GetComponent<Parcel>().SetRoute(route);
		}
	}

	void Update()
	{
		if (parcelsStolen == 4)
		{
			
		}
	}

	public static void AddScore(int score)
	{
		instance.score += score;
		instance.scoreText.text = instance.score.ToString();
		instance.audio.Play();
	}

	public static void DeliverParcel(Parcel parcel)
	{
		AddScore(parcel.GetPoints() * instance.player.CarryCount());

		// Respawn parcel
		DeliveryRoute route = parcel.GetRoute();

		GameObject go = Instantiate(instance.parcelPrefab, route.parcelSpawnPoint.position, Quaternion.identity);
		go.GetComponent<Parcel>().SetRoute(route);
	}

	public static void UpdateIndicators(List<Parcel> parcels)
	{
		// Turn off all indicators
		foreach (DeliveryRoute route in instance.deliveryRoutes)
		{
			route.indicator.gameObject.SetActive(false);
		}

		// Turn on active indicators
		foreach (Parcel parcel in parcels)
		{
			parcel.GetRoute().indicator.gameObject.SetActive(true);
		}
	}
}