using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
	[SerializeField]
	private Transform target;
	[SerializeField]
	private float width = 50f;

	void Update()
	{
		Vector3 pos = Camera.main.WorldToScreenPoint(target.position);
		pos.x = Mathf.Clamp(pos.x, width, Camera.main.pixelWidth - width);
		pos.y = Mathf.Clamp(pos.y, width, Camera.main.pixelHeight - width);
		transform.position = pos;
	}
}