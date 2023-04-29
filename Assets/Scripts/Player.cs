using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed = 5;

	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		Vector3 move = Vector3.zero;

		if (Input.GetKey(KeyCode.W))
		{
			move.z += 1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			move.z -= 1;
		}
		if (Input.GetKey(KeyCode.A))
		{
			move.x -= 1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			move.x += 1;
		}

		move.Normalize();

		rb.velocity = move * moveSpeed;
	}
}