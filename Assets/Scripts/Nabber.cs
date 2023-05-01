using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Nabber : MonoBehaviour, IStackable
{
	[SerializeField]
	private Vector3 stackingPoint = Vector3.up * 0.5f;
	[SerializeField]
	private Quaternion stackingRotation = Quaternion.identity;
	[SerializeField]
	private GameObject damagePrefab;

	private NavMeshAgent agent;
	private Transform target;

	private float thinkTimer = 0;
	private float hurtTimer = 0;
	private Parcel heldParcel = null;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		if (hurtTimer > 0)
		{
			hurtTimer -= Time.deltaTime;
		}
		else if (thinkTimer > 0)
		{
			thinkTimer -= Time.deltaTime;
		}
		else
		{
			thinkTimer = 0.5f;

			// Check for parcels to grab
			if (heldParcel == null)
			{
				List<Parcel> parcels = GetComponentInChildren<ParcelPickup>().parcels;
				if (parcels.Count > 0)
				{
					parcels[0].Pickup(this, transform);
					heldParcel = parcels[0];
				}
			}

			// Pathfind
			GameObject[] targets = GameObject.FindGameObjectsWithTag(heldParcel ? "Burrow" : "Parcel");

			Transform closestTarget = null;
			float closestDist = float.PositiveInfinity;

			foreach (GameObject target in targets)
			{
				float dist = Vector3.Distance(transform.position, target.transform.position);
				if (dist < closestDist)
				{
					closestDist = dist;
					closestTarget = target.transform;
				}
			}

			// Attack if nearby
			if (closestDist < 1f)
			{
				if (heldParcel == null)
				{
					Damage damage = Instantiate(damagePrefab, transform.position, Quaternion.identity).GetComponent<Damage>();
					damage.damageType = Damage.DamageType.Player;
				}
				else
				{
					Destroy(heldParcel.gameObject);
					heldParcel = null;
				}
			}

			target = closestTarget;
		}



		if (target != null)
		{
			agent.destination = target.position;
		}
	}

	public Vector3 GetStackingPoint()
	{
		return stackingPoint;
	}

	public Quaternion GetStackingRotation()
	{
		return stackingRotation;
	}

	public void RemoveFromList(IStackable item)
	{
		heldParcel = null;
	}

	public void Hurt()
	{
		hurtTimer = 1.5f;
		agent.destination = transform.position;

		if (heldParcel)
		{
			heldParcel.Drop();
			heldParcel = null;
		}
	}
}