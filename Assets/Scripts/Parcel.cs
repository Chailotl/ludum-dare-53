using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcel : MonoBehaviour, IStackable
{
	[SerializeField]
	private Transform heldBy;
	[SerializeField]
	private Transform destination;

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
			transform.position += new Vector3(Mathf.PerlinNoise(Time.time / 2f + seed, 0) - 0.5f, 0, Mathf.PerlinNoise(Time.time / 2f + seed, 100) - 0.5f) / 4f;
		}
	}

	public Vector3 GetStackingPoint()
	{
		return Vector3.up * transform.localScale.x;
	}

	public void Pickup(Transform anchor)
	{
		heldBy = anchor;
		gameObject.layer = LayerMask.NameToLayer("Carried");
		rb.isKinematic = true;
		transform.rotation = Quaternion.identity;
	}

	public void Drop()
	{
		heldBy = null;
		gameObject.layer = LayerMask.NameToLayer("Default");
		rb.isKinematic = false;
		
		// Random vel
		Vector3 vel = Random.insideUnitSphere * 2f;
		vel.y = Random.Range(2f, 3f);

		rb.velocity = vel;
	}
}