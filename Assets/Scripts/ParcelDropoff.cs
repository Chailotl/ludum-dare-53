using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcelDropoff : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Parcel parcel;
		if (parcel = other.GetComponent<Parcel>())
		{
			GameManager.AddScore(parcel.GetPoints());
			Destroy(parcel.gameObject);
		}
	}
}