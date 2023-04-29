using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour, IStackable
{
	[SerializeField]
	private Transform heldBy;

	private float seed;

	void Start()
	{
		seed = Random.value * 100f;
	}

	void FixedUpdate()
	{
		if (heldBy != null && heldBy.GetComponent<IStackable>() != null)
		{
			// anchor point
			transform.position = heldBy.position + heldBy.GetComponent<IStackable>().GetStackingPoint();

			// random shake
			transform.position += new Vector3(Mathf.PerlinNoise(Time.time / 2f + seed, 0) - 0.5f, 0, Mathf.PerlinNoise(Time.time / 2f + seed, 100) - 0.5f) / 4f;
		}
	}

	public Vector3 GetStackingPoint()
	{
		return Vector3.up * transform.localScale.x;
	}
}