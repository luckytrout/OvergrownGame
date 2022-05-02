using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectGravity : MonoBehaviour
{

	private PlanetGravity attractor;
	private Rigidbody rb;

	public bool placeOnSurface = false;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		attractor = PlanetGravity.instance;
	}

	void FixedUpdate()
	{
		if (placeOnSurface)
			attractor.PlaceOnSurface(rb);
		else
			attractor.Attract(rb);
	}

}
