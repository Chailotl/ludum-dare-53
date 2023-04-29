using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcelPickup : MonoBehaviour
{
	public List<Parcel> parcels = new List<Parcel>();

	private void OnTriggerEnter(Collider other)
	{
		Parcel parcel;
		if (parcel = other.GetComponent<Parcel>())
		{
			parcels.Add(parcel);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Parcel parcel;
		if (parcel = other.GetComponent<Parcel>())
		{
			parcels.Remove(parcel);
		}
	}
}