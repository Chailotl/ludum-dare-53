using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed = 5;
	[SerializeField]
	private float accel = 25;

	private Rigidbody rb;
	private Vector3 move = Vector3.zero;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		Vector3 input = Vector3.zero;

		if (Input.GetKey(KeyCode.W))
		{
			input.z += 1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			input.z -= 1;
		}
		if (Input.GetKey(KeyCode.A))
		{
			input.x -= 1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			input.x += 1;
		}

		input = input.normalized * moveSpeed;

		move = Vector3.MoveTowards(move, input, accel * Time.deltaTime);

		rb.velocity = move;
	}
}