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
	private Vector3 stackingPoint = Vector3.up * 0.5f;

	// Input
	private Vector2 move = Vector2.zero;

	// Physics
	private Rigidbody rb;
	private Vector3 vel = Vector3.zero;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		vel = Vector3.MoveTowards(vel, new Vector3(move.x, 0, move.y), accel * Time.deltaTime);

		rb.velocity = vel;
	}

	public Vector3 GetStackingPoint()
	{
		return stackingPoint;
	}

	private void OnMove(InputValue val)
	{
		move = val.Get<Vector2>() * moveSpeed;
	}
}