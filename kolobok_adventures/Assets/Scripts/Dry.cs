using UnityEngine;
using System.Collections;

public class Dry : MonoBehaviour {
	void OnTriggerEnter2D (Collider2D other)
	{
		GameObject k = GameObject.Find ("Kolobok");
		Debug.Log ("Trigger entered " + other);
		Debug.Log ("Inertia " + other.attachedRigidbody.inertia);
		k.collider2D.attachedRigidbody.fixedAngle = false;
		k.collider2D.sharedMaterial.friction = 500;
		//k.collider2D.attachedRigidbody.inertia = 10;
		other.sharedMaterial.bounciness = 0;
		k.collider2D.enabled = false;
		k.collider2D.enabled = true;
		//other.attachedRigidbody.angularDrag = 0.2f;
		//other.attachedRigidbody.inertia = 10000;
		//other.attachedRigidbody.AddForce (new Vector2(300,0));
	}
}
