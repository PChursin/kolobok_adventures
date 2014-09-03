using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {
	bool s = false;
	bool slipSet = false;
	bool drySet = false;
	GameObject dragging = null;
	private Vector3 lastMouse;

	void Start()
	{
		GameObject k = GameObject.Find ("Kolobok");
		k.collider2D.sharedMaterial.friction = 5;
		k.collider2D.enabled = false;
		k.collider2D.enabled = true;
		lastMouse = Input.mousePosition;
	}

	void OnGUI()
	{
		if (Input.GetMouseButton (0))
			dragging = null;

		if (s == false && slipSet && drySet)
		{
			if (GUI.Button(new Rect(Screen.width/2-50, 20, 100, 40), "Start")) {
				s = true;	
				GameObject.Find("Kolobok").rigidbody2D.WakeUp();
			}			
		}

		if (slipSet == false)
		{
			if (GUI.Button(new Rect(Screen.width/2-60, 20, 120, 40), "Set slippery region")) {
				slipSet = true;	
				dragging = GameObject.Find("slip");
			}
		}

		if (drySet == false)
		{
			if (GUI.Button(new Rect(Screen.width/2-60, 80, 120, 40), "Set dry region")) {
				drySet = true;	
				dragging = GameObject.Find("dry");
			}
		}
	}

	void Update()
	{
		if (lastMouse != Input.mousePosition)
		{
			if (dragging != null)
			{
				Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				dragging.transform.position = new Vector3(mouse.x, mouse.y, 0);
				//dragging.transform.position = 
				//Instantiate(dragging, Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.rotation);
				//dragging.transform.localPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
		}

	}

	void OnMouseDown()
	{
		dragging = null;
	}
}
