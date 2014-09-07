using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoftBodyCreator : MonoBehaviour {
	public List<GameObject> spheres;
	public List<GameObject> capsules;
	GameObject center;
	public float delta = 0.3f;
	public float h = 0;

	// Use this for initialization
	void Start () {			
		center = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		Component.DestroyImmediate (center.GetComponent<SphereCollider> ());
		center.AddComponent<CircleCollider2D> ();

		Rigidbody2D cBody = center.AddComponent<Rigidbody2D> ();
		cBody.mass = 5f;
		//cBody.isKinematic = true;

		center.transform.parent = gameObject.transform;
		center.transform.localPosition = new Vector3 ();

		spheres = new List<GameObject> ();

		float angle = 0;
		float R = 2;

		while (angle < Mathf.PI * 2) {
			GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Component.DestroyImmediate (g.GetComponent<SphereCollider>());
			Rigidbody2D body = g.AddComponent<Rigidbody2D>();
			body.mass = 1f;				

			DistanceJoint2D distanceJoint = g.AddComponent<DistanceJoint2D>();
			distanceJoint.maxDistanceOnly = true;
			distanceJoint.connectedBody = cBody;
			distanceJoint.distance = R;

			Spring s = g.AddComponent<Spring>();
			s.connectedBody = cBody;
			s.k = 400;
			s.distance = R;

			g.transform.localScale = new Vector3(0.2f, 0.2f, 1);
			g.transform.parent = center.transform;
			g.transform.localPosition = new Vector3(R * Mathf.Cos(angle), R * Mathf.Sin(angle), 0);

			g.AddComponent<CircleCollider2D>();

			spheres.Add(g);
			angle += delta;
		}


		h = 2 * R * Mathf.Sin (delta / 2);

		for (int i = 0; i < spheres.Count; i++) {
			DistanceJoint2D distanceJoint = spheres[i].AddComponent<DistanceJoint2D>();
			distanceJoint.connectedBody = spheres[(i - 1 + spheres.Count) % spheres.Count]
											.GetComponent<Rigidbody2D>();
			distanceJoint.distance = h;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
