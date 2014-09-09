using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;

public class SoftBodyCreator : MonoBehaviour {
	public List<GameObject> spheres;
	public List<GameObject> capsules;
	GameObject center;
	public float h = 0;

	// Use this for initialization
	void Start () {			
		var holder = new GameObject ();
		holder.transform.position = new Vector3 ();
		holder.name = "KOLOBOK";

		center = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		Component.DestroyImmediate (center.GetComponent<SphereCollider> ());
		center.AddComponent<CircleCollider2D> ();
		center.transform.parent = center.transform;
		center.transform.localPosition = gameObject.transform.position;
		center.transform.localPosition = new Vector3 ();
		center.transform.localScale = new Vector3 (0.2f, 0.2f, 1);

		Rigidbody2D cBody = center.AddComponent<Rigidbody2D> ();
		cBody.mass = 1f;
		//cBody.isKinematic = true;	

		spheres = new List<GameObject> ();

		float delta = Mathf.PI * 2 / 20;
		float angle = 0;
		float R = 1;



		while (angle < Mathf.PI * 2) {
			GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			g.transform.transform.position = new Vector3(R * Mathf.Cos(angle), R * Mathf.Sin(angle), 0)
											+ center.transform.position;
			g.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			g.transform.parent = holder.transform;

			Component.DestroyImmediate (g.GetComponent<SphereCollider>());
			Rigidbody2D body = g.AddComponent<Rigidbody2D>();
			body.mass = 1f;				

			DistanceJoint2D distanceJoint = g.AddComponent<DistanceJoint2D>();
			distanceJoint.maxDistanceOnly = true;
			distanceJoint.connectedBody = cBody;
			distanceJoint.distance = R;


			Spring s = g.AddComponent<Spring>();
			s.connectedBody = cBody;
			s.k = 300;
			s.distance = R;		

			CircleCollider2D cc = g.AddComponent<CircleCollider2D>();
			cc.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Materials/KolobokMat"); // Не работает!
			cc.enabled = false;
			cc.enabled = true;

			spheres.Add(g);
			angle += delta;
		}


		h = Mathf.Sqrt(2 * R * R * (1 - Mathf.Cos (delta)))*0.99f;
		Debug.Log (h * 10 - Mathf.PI * R);
		angle = 0;
		for (int i = 0; i < spheres.Count; i++) {

			GameObject dummy = new GameObject();
			dummy.transform.parent = spheres[i].transform;
			dummy.transform.localPosition = new Vector3();
			var dBody = dummy.AddComponent<Rigidbody2D>();
			dBody.mass = 0;
		

			var dHingeJoint = dummy.AddComponent<HingeJoint2D>();
			dHingeJoint.useLimits = true;
			dHingeJoint.connectedBody = spheres[i].GetComponent<Rigidbody2D>();

			/*
			var connection = spheres[i].AddComponent<Spring>();
			connection.connectedBody = spheres[(i - 1 + spheres.Count) % spheres.Count]
										.GetComponent<Rigidbody2D>();
			connection.distance = h;	
			connection.k = 1000;

			*/
			DistanceJoint2D distanceJoint = spheres[i].AddComponent<DistanceJoint2D>();
			distanceJoint.connectedBody = spheres[(i - 1 + spheres.Count) % spheres.Count]
											.GetComponent<Rigidbody2D>();
			distanceJoint.distance = h;		


			angle += delta;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
