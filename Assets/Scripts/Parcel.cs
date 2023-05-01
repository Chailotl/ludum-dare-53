using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcel : MonoBehaviour, IStackable
{
	[SerializeField]
	private Vector3 stackingPoint = Vector3.up * 0.5f;
	[SerializeField]
	private SkinnedMeshRenderer render;
	[SerializeField]
	private List<Material> mats = new List<Material>();

	public bool Damaged { get; private set; }

	private Rigidbody rb;
	private float seed;
	private GameManager.DeliveryRoute route;
	public bool delivered = false;

	private IStackable holder;
	private Transform anchor;
	public Parcel aboveStack;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		seed = Random.value * 100f;
		render.material = mats[Random.Range(0, mats.Count)];
	}

	/*void Update()
	{
		IStackable stack;
		if (anchor != null && (stack = anchor.GetComponent<IStackable>()) != null)
		{
			//if (stack != holder) { return; }

			// Anchor point
			transform.position = anchor.position + stack.GetStackingPoint();

			// Random shake
			transform.position += new Vector3(Mathf.PerlinNoise(Time.time / 3f + seed, 0) - 0.5f, 0, Mathf.PerlinNoise(Time.time / 3f + seed, 100) - 0.5f) / 6f;
		}
	}*/

	void FixedUpdate()
	{
		IStackable stack;
		if (anchor != null && (stack = anchor.GetComponent<IStackable>()) != null)
		{
			// Anchor point
			Vector3 pos = anchor.position + stack.GetStackingPoint();

			Quaternion q = holder.GetStackingRotation();
			if (q != Quaternion.identity)
			{
				transform.rotation = q;
			}

			// Random shake
			if (stack != holder)
			{
				pos += new Vector3(Mathf.PerlinNoise(Time.time / 3f + seed, 0) - 0.5f, 0, Mathf.PerlinNoise(Time.time / 3f + seed, 100) - 0.5f) / 6f;
			}


			rb.MovePosition(pos);
		}
	}

	public Vector3 GetStackingPoint()
	{
		return stackingPoint;
	}

	public void SetRoute(GameManager.DeliveryRoute route)
	{
		this.route = route;
	}

	public GameManager.DeliveryRoute GetRoute()
	{
		return route;
	}

	public IStackable GetHolder()
	{
		return holder;
	}

	public void Pickup(IStackable holder, Transform anchor)
	{
		this.holder = holder;
		this.anchor = anchor;

		Parcel parcel = anchor.GetComponent<Parcel>();
		if (parcel)
		{
			parcel.aboveStack = this;
		}

		gameObject.tag = "Parcel";
		gameObject.layer = LayerMask.NameToLayer("Carried Parcel");
		rb.isKinematic = true;


		// Apply rotation
		transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
	}

	public void Drop()
	{
		// Clear anchor's aboveStack
		Parcel parcel = anchor.GetComponent<Parcel>();

		if (parcel)
		{
			parcel.aboveStack = null;
		}

		// Check if there is a parcel above and shift it down
		if (aboveStack != null)
		{
			aboveStack.Pickup(holder, anchor);
		}

		if (holder != null)
		{
			holder.RemoveFromList(this);
		}

		holder = null;
		anchor = null;
		aboveStack = null;


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
		int points = route.points;

		if (Damaged) { points /= 2; }

		return points;
	}

	public void RemoveFromList(IStackable item)
	{
		throw new System.NotImplementedException();
	}
}