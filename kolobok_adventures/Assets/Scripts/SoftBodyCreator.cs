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
		cBody.mass = 5;
		center.transform.parent = gameObject.transform;
		center.transform.localPosition = new Vector3 ();

		spheres = new List<GameObject> ();

		float angle = 0;
		float R = 1.2f;

		while (angle < Mathf.PI * 2) {
			GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Component.DestroyImmediate (g.GetComponent<SphereCollider>());
			Rigidbody2D body = g.AddComponent<Rigidbody2D>();
			body.mass = 0.01f;				


			SpringJoint2D sprintJoint = g.AddComponent<SpringJoint2D> ();
			sprintJoint.connectedBody = cBody;
			sprintJoint.distance = R;
			sprintJoint.frequency = 10;
			sprintJoint.dampingRatio = 0f;


			SliderJoint2D sliderJoint = g.AddComponent<SliderJoint2D>();
			sliderJoint.angle = angle * Mathf.Rad2Deg;
			sliderJoint.useLimits = true;
			sliderJoint.collideConnected = true;
		
			JointTranslationLimits2D jtl = new JointTranslationLimits2D();
			jtl.min = 0.1f;
			jtl.max = R;
			sliderJoint.limits = jtl;

			sliderJoint.connectedBody = cBody;	

			g.transform.localScale = new Vector3(0.2f, 0.2f, 1);
			g.transform.parent = center.transform;
			g.transform.localPosition = new Vector3(R * Mathf.Cos(angle), R * Mathf.Sin(angle), 0);

			g.AddComponent<CircleCollider2D>();


			spheres.Add(g);
			angle += delta;
		}

		h = 2 * R * Mathf.Sin (delta / 2);
		for (int i = 0; i < spheres.Count; i++) {
			GameObject dummy = new GameObject();
			Rigidbody2D dBody = dummy.AddComponent<Rigidbody2D>();
			dBody.mass = 0;
			dummy.transform.parent = spheres[i].transform;
			dummy.transform.localPosition = new Vector3();

			HingeJoint2D hingeJoint = dummy.AddComponent<HingeJoint2D>();
			hingeJoint.connectedBody = spheres[i].GetComponent<Rigidbody2D>();

			SpringJoint2D springJoint = dummy.AddComponent<SpringJoint2D>();
			springJoint.connectedBody = spheres[ (i + spheres.Count - 1) % spheres.Count]
										.GetComponent<Rigidbody2D>();
			springJoint.distance = h;
			springJoint.frequency = 100;
			springJoint.dampingRatio = 0.5f;
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
