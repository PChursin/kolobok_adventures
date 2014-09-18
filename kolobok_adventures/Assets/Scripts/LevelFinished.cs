using UnityEngine;
using System.Collections;

public class LevelFinished : MonoBehaviour {		
	bool isFinished = false;
	
	void OnTriggerEnter2D (Collider2D other) {	
		if(other.name == "KOLOBOK_CENTER") {
			isFinished = true;		
			GameObject.Find("KOLOBOK_CENTER").GetComponent<Rigidbody2D>().isKinematic = true;
		}
	}
	
	void OnGUI() {
		if(isFinished) {
			int width = 200;
			int height = 120;
			if(GUI.Button(
				new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height),
				"Level finished! Next level"
			)) {
				Application.LoadLevel(Application.loadedLevel + 1);
			}		
		}		
	}
	
}
