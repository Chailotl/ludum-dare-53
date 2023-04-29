using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcel : MonoBehaviour, IStackable
{
	public Transform heldBy;

	private float seed;

	void Start()
	{
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
}