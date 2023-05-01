using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Nabber : MonoBehaviour, IStackable
{
	[SerializeField]
	private Vector3 stackingPoint = Vector3.up * 0.5f;

	private NavMeshAgent agent;

	private float thinkTimer = 0;
	private float hurtTimer = 0;
	private Parcel heldParcel = null;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		if (thinkTimer > 0)
		{
			thinkTimer -= Time.deltaTime;
		}
		else
		{
			thinkTimer = 0.5f;

			// Check for parcels to grab
			List<Parcel> parcels = GetComponentInChildren<ParcelPickup>().parcels;
			if (parcels.Count > 0)
			{
				parcels[0].Pickup(this, transform);
				heldParcel = parcels[0];
			}

			GameObject[] targets = GameObject.FindGameObjectsWithTag(heldParcel ? "Burrow" : "Parcel");

			GameObject closestTarget = null;
			float closestDist = float.PositiveInfinity;

			foreach (GameObject target in targets)
			{
				float dist = Vector3.Distance(transform.position, target.transform.position);
				if (dist < closestDist)
				{
					closestDist = dist;
					closestTarget = target;
				}
			}

			agent.destination = closestTarget.transform.position;
		}
	}

	public Vector3 GetStackingPoint()
	{
		return stackingPoint;
	}

	public void RemoveFromList(IStackable item)
	{
		heldParcel = null;
	}

	public void Hurt()
	{
		hurtTimer = 0.5f;
		if (heldParcel)
		{
			heldParcel.Drop();
			heldParcel = null;
		}
	}
}