using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IStackable
{
	[SerializeField]
	private float moveSpeed = 5;
	[SerializeField]
	private float accel = 25;

	[SerializeField]
	private Animator animator;
	[SerializeField]
	private SpriteRenderer render;
	[SerializeField]
	private Vector3 stackingPoint = Vector3.up * 0.5f;

	private List<Parcel> carrying = new List<Parcel>();

	// Input
	private Vector2 moving = Vector2.zero;

	// Physics
	private Rigidbody rb;
	private Vector3 vel = Vector3.zero;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		vel = Vector3.MoveTowards(vel, new Vector3(moving.x, 0, moving.y), accel * Time.deltaTime);

		vel.y = rb.velocity.y;
		rb.velocity = vel;
		vel.y = 0;

		// Animate character
		float speed = vel.sqrMagnitude;
		bool carryingParcels = carrying.Count > 0;

		if (speed > 0.05f && (moving.x != 0 || moving.y != 0))
		{
			animator.Play(carryingParcels ? "Bun Carry Walk" : "Bun Walk");
		}
		else
		{
			animator.Play(carryingParcels ? "Bun Carry Idle" : "Bun Idle");
		}

		// Sprite flipper
		if (moving.x > 0)
		{
			render.flipX = false;
		}
		else if (moving.x < 0)
		{
			render.flipX = true;
		}
	}

	public void DropParcel()
	{
		if (carrying.Count == 0) { return; }

		Parcel parcel = carrying[carrying.Count - 1];

		carrying.Remove(parcel);
		parcel.Drop();
	}

	public Vector3 GetStackingPoint()
	{
		return stackingPoint;
	}

	private void OnMove(InputValue val)
	{
		moving = val.Get<Vector2>() * moveSpeed;
	}

	// Pick up all parcels in range
	private void OnPickup()
	{
		List<Parcel> parcels = GetComponentInChildren<ParcelPickup>().parcels;

		foreach (Parcel parcel in parcels)
		{
			if (!carrying.Contains(parcel))
			{
				if (carrying.Count == 0)
				{
					parcel.Pickup(transform);
				}
				else
				{
					parcel.Pickup(carrying[carrying.Count - 1].transform);
				}

				carrying.Add(parcel);
			}
		}
	}

	// Drop one parcel
	private void OnDrop()
	{
		DropParcel();
	}
}