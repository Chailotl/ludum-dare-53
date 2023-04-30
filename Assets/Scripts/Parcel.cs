using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcel : MonoBehaviour, IStackable
{
	[SerializeField]
	private Transform heldBy;
	[SerializeField]
	private Transform destination;
	[SerializeField]
	private Vector3 stackingPoint = Vector3.up * 0.5f;
	[SerializeField]
	private SkinnedMeshRenderer render;

	public bool Damaged { get; private set; }

	private Rigidbody rb;
	private float seed;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		seed = Random.value * 100f;
	}

	void FixedUpdate()
	{
		IStackable stack;
		if (heldBy != null && (stack = heldBy.GetComponent<IStackable>()) != null)
		{
			// Anchor point
			transform.position = heldBy.position + stack.GetStackingPoint();

			// Random shake
			transform.position += new Vector3(Mathf.PerlinNoise(Time.time / 3f + seed, 0) - 0.5f, 0, Mathf.PerlinNoise(Time.time / 3f + seed, 100) - 0.5f) / 6f;
		}
	}

	public Vector3 GetStackingPoint()
	{
		return stackingPoint;
	}

	public void Pickup(Transform anchor)
	{
		heldBy = anchor;
		gameObject.layer = LayerMask.NameToLayer("No Collide");
		rb.isKinematic = true;
		transform.rotation = Quaternion.identity;
	}

	public void Drop()
	{
		heldBy = null;
		gameObject.layer = LayerMask.NameToLayer("Parcel");
		rb.isKinematic = false;
		
		// Random vel
		Vector3 vel = Random.insideUnitSphere * 2f;
		vel.y = Random.Range(2f, 3f);

		rb.velocity = vel;
	}

	public void Damage()
	{
		if (Damaged)
		{
			Destroy(gameObject);
			return;
		}

		Damaged = true;
		render.SetBlendShapeWeight(0, 100);
	}

	public int GetPoints()
	{
		int points = 10;

		if (Damaged) { points /= 2; }

		return points;
	}
}