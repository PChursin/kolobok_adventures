using UnityEngine;
using System.Collections;

public class Slippery : MonoBehaviour {
	void OnTriggerEnter2D (Collider2D other)
	{
		GameObject k = GameObject.Find ("Kolobok");
		Debug.Log ("Trigger entered " + other);
		//other.attachedRigidbody.drag = 0;
		//other.attachedRigidbody.fixedAngle = true;
		//other.attachedRigidbody.angularDrag = 0.01f;

		k.collider2D.sharedMaterial.friction = 0.01f;
		k.collider2D.enabled = false;
		k.collider2D.enabled = true;
		//other.attachedRigidbody.AddForce (new Vector2(300,0));
	}
}
