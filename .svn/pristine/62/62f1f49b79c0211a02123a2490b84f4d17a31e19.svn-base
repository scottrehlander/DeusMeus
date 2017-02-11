using UnityEngine;
using System.Collections;

public class Camera_follow : MonoBehaviour {

	public Transform target;
	public Transform cameraBounds;
	public float zoom;

	float width;
	float height;

	// Use these some day if we want smooth camera movement
	//Vector3 velocity = Vector3.zero;
	//float dampTime = 0.15f;

	// Use this for initialization
	void Start () {
		width = cameraBounds.Find ("TopRight").transform.position.x - cameraBounds.Find ("TopLeft").transform.position.x;
		height = cameraBounds.Find ("TopLeft").transform.position.x - cameraBounds.Find ("BottomRight").transform.position.x;
		camera.orthographicSize = zoom;
		transform.position = target.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Comma)) {
			zoom += .02F;
			camera.orthographicSize = zoom;
		} 
		if (Input.GetKey(KeyCode.Period)) {
			zoom -= .02F;
			camera.orthographicSize = zoom;
		} 

		float camHeight = (2f * camera.orthographicSize) / 2F;
		float camWidth = (camHeight * 2 * camera.aspect) / 2F;

		var camPos = new Vector3 (
			Mathf.Clamp (target.position.x, 0 + camWidth, width - camWidth),
			Mathf.Clamp (target.position.y, height + camHeight, 0 - camHeight),
			zoom * -1
		);

		// Smooth scrolling is cool, but let's do regular until we have more time
		//transform.position = Vector3.SmoothDamp(transform.position, camPos, ref velocity, dampTime);
		transform.position = camPos;
	}
}
