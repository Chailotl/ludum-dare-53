using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStackable
{
	public Vector3 GetStackingPoint();
	public Quaternion GetStackingRotation()
	{
		return Quaternion.identity;
	}
	public void RemoveFromList(IStackable item);
}