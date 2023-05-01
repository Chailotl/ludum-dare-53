using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcelDropoff : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Parcel parcel = other.GetComponent<Parcel>();
		if (parcel != null && parcel.GetRoute().destination == gameObject && parcel.delivered == false)
		{
			GameManager.AddScore(parcel.GetPoints());
			GameManager.DeliverParcel(parcel);
			parcel.Drop(true);
			Destroy(parcel.gameObject, 5f);
		}
	}
}