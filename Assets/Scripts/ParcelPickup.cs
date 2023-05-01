using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcelPickup : MonoBehaviour
{
	public List<Parcel> parcels = new List<Parcel>();
	[SerializeField]
	private bool ignoreUntagged = false;

	private void OnTriggerEnter(Collider other)
	{
		Parcel parcel = other.GetComponent<Parcel>();
		if (parcel && !parcel.delivered)
		{
			if (!ignoreUntagged || (ignoreUntagged && parcel.tag == "Parcel"))
			{
				parcels.Add(parcel);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Parcel parcel = other.GetComponent<Parcel>();
		if (parcel)
		{
			parcels.Remove(parcel);
		}
	}
}