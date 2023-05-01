using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
	public enum DamageType { Player, Enemy, Parcel };

	public DamageType damageType;

	private void OnTriggerEnter(Collider other)
	{
		if (damageType == DamageType.Player)
		{
			Player player = other.GetComponent<Player>();
			if (player != null)
			{
				player.Hurt();
			}
		}
		else if (damageType == DamageType.Parcel)
		{
			Parcel parcel = other.GetComponent<Parcel>();
			if (parcel != null)
			{
				parcel.Damage();
			}
		}
		else if (damageType == DamageType.Enemy)
		{
			// funny :)
			/*Parcel parcel = other.GetComponent<Parcel>();
			if (parcel != null)
			{
				parcel.Damage();
				parcel.GetComponent<Rigidbody>().velocity = new Vector3(-15, 5, 0);
			}*/
		}

		Destroy(gameObject);
	}
}