using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
  public float xShift = -10f;   // Сдвиг по осям
  public float yShift = 10f;
	public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
	public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.
	
	private GameObject player;
	private Transform playerTransform;		// Reference to the player's transform.
	private bool init = false;
  
	public void initCam ()
	{
		// Setting up the reference.
		player = GameObject.Find("KOLOBOK_CENTER");
    Debug.Log (player);
    playerTransform = player.transform;
    init = true;
	}
	
	
	bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the playerTransform in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - playerTransform.position.x + xShift) > xMargin;
	}
	
	
	bool CheckYMargin()
	{
		// Returns true if the distance between the camera and the playerTransform in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - playerTransform.position.y + yShift) > yMargin;
	}
	
	
	void FixedUpdate ()
	{
    if (init)
      TrackplayerTransform();
	}
	
	
	void TrackplayerTransform ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;
		
		// If the playerTransform has moved beyond the x margin...
		if(CheckXMargin())
			// ... the target x coordinate should be a Lerp between the camera's current x position and the playerTransform's current x position.
			targetX = Mathf.Lerp(transform.position.x, playerTransform.position.x + xShift, xSmooth * Time.deltaTime);
		
		// If the playerTransform has moved beyond the y margin...
		if(CheckYMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and the playerTransform's current y position.
			targetY = Mathf.Lerp(transform.position.y, playerTransform.position.y + yShift, ySmooth * Time.deltaTime);
		
		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);
		
		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
