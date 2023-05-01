using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IStackable
{
	[SerializeField]
	private Transform target;
	[SerializeField]
	private Vector3 stackingPoint = Vector3.up * 0.5f;

	private NavMeshAgent agent;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		agent.destination = target.position;
	}

	public Vector3 GetStackingPoint()
	{
		return stackingPoint;
	}

	public void RemoveFromList(IStackable item)
	{
		throw new System.NotImplementedException();
	}
}