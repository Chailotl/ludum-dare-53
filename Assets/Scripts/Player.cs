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
	private float carrySpeedLoss = 0.5f;
	[SerializeField]
	private float accel = 25;

	[SerializeField]
	private GameObject damagePrefab;

	[SerializeField]
	private Animator animator;
	[SerializeField]
	private SpriteRenderer render;
	[SerializeField]
	private Vector3 stackingPoint = Vector3.up * 0.5f;

	private float attackTimer = 0;
	private bool waitingToAttack = false;
	private float hurtTimer = 0;

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

	void FixedUpdate()
	{
		Vector2 speed = moving * (moveSpeed - carrying.Count * carrySpeedLoss);
		if (attackTimer > 0 || hurtTimer > 0) { speed *= 0.25f; }

		vel = Vector3.MoveTowards(vel, new Vector3(speed.x, 0, speed.y), accel * Time.deltaTime);

		vel.y = rb.velocity.y;
		rb.velocity = vel;
		vel.y = 0;

		// Animate character
		bool carryingParcels = carrying.Count > 0;

		if (attackTimer > 0)
		{
			attackTimer -= Time.deltaTime;

			if (waitingToAttack && attackTimer < 25f / 60f)
			{
				waitingToAttack = false;
				Vector3 side = render.flipX ? Vector3.left : Vector3.right;
				Damage damage = Instantiate(damagePrefab, transform.position + side, Quaternion.identity).GetComponent<Damage>();
				damage.damageType = Damage.DamageType.Enemy;
			}
		}
		else if (hurtTimer > 0)
		{
			hurtTimer -= Time.deltaTime;
			animator.Play("Bun Hurt");
		}
		else if (vel.sqrMagnitude > 0.05f && (moving.x != 0 || moving.y != 0))
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

		// Stick to ground
		RaycastHit hit;
		if (Physics.Raycast(new Ray(transform.position, Vector3.down ), out hit, 0.51f, 1, QueryTriggerInteraction.Ignore))
		{
			transform.position = hit.point + Vector3.up * 0.5f;

			Vector3 v = rb.velocity;
			v.y = 0;
			rb.velocity = v;
		}
	}

	public void DropParcel()
	{
		if (carrying.Count == 0) { return; }

		Parcel parcel = carrying[carrying.Count - 1];

		carrying.Remove(parcel);
		UpdateIndicators();
		parcel.Drop();
	}

	public void Hurt()
	{
		hurtTimer = 0.5f;
		int count = carrying.Count;
		for (int i = 0; i < count; ++i)
		{
			DropParcel();
		}
	}

	public Vector3 GetStackingPoint()
	{
		return stackingPoint;
	}

	public void RemoveFromList(IStackable item)
	{
		if (item is Parcel)
		{
			carrying.Remove(item as Parcel);
			UpdateIndicators();
		}
	}

	private void OnMove(InputValue val)
	{
		moving = val.Get<Vector2>();
	}

	// Pick up all parcels in range
	private void OnPickup()
	{
		List<Parcel> parcels = GetComponentInChildren<ParcelPickup>().parcels;

		foreach (Parcel parcel in parcels)
		{
			if (!carrying.Contains(parcel) && !parcel.delivered)
			{
				if (carrying.Count == 0)
				{
					parcel.Pickup(this, transform);
				}
				else
				{
					parcel.Pickup(this, carrying[carrying.Count - 1].transform);
				}

				carrying.Add(parcel);
				UpdateIndicators();
			}
		}
	}

	// Drop one parcel
	private void OnDrop()
	{
		DropParcel();
	}

	private void OnAttack()
	{
		if (carrying.Count == 0 && attackTimer <= 0)
		{
			animator.Play("Bun Punch");
			attackTimer = 40f / 60f;
			waitingToAttack = true;
		}
	}

	private void UpdateIndicators()
	{
		GameManager.UpdateIndicators(carrying);
	}

	public int CarryCount()
	{
		return carrying.Count;
	}
}