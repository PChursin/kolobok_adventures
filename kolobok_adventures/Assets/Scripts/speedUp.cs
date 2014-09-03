using UnityEngine;
using System.Collections;

public class speedUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Debug.Log ("Init" + this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("Trigger entered " + other);
		//other.attachedRigidbody.fixedAngle = true;
		other.attachedRigidbody.drag = 0.01f;
		//other.attachedRigidbody.AddForce (new Vector2(300,0));
	}
}
