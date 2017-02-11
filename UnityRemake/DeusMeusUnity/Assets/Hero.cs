using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : MonoBehaviour {

	string spawnPosition = "";

	Animator anim;
	float movespeed = .03f;

	bool moveBlocked_left = false;
	bool moveBlocked_right = false;
	bool moveBlocked_up = false;
	bool moveBlocked_down = false;
	List<Collider2D> activeCollisions;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		activeCollisions = new List<Collider2D> ();

		// Set the spawn point
		spawnPosition = PlayerPrefs.GetString ("SpawnPosition");
		if (spawnPosition.Equals(string.Empty)) {
			spawnPosition = "StartGame";
		}

		var spawnPoint = GameObject.Find ("/PlayerSpawns/" + spawnPosition);
		if (spawnPoint != null) {
			transform.position = spawnPoint.transform.position;
		}
	}

	// If the block boolean is false then we no longer want to block
	private void BlockMovement(string direction, bool block)
	{
		if(direction.Equals("Right"))
			moveBlocked_right = block;
		else if(direction.Equals("Left"))
			moveBlocked_left = block;
		else if(direction.Equals("Up"))
			moveBlocked_up = block;
		else if(direction.Equals("Down"))
			moveBlocked_down = block;
	}

	public void CollisionDetected(string direction, Collider2D collider)
	{
		if(Utilities.ShouldColliderBlock(collider)) {
			BlockMovement (direction, true);
		}
		activeCollisions.Add (collider);
	}

	public void CollisionClear(string direction, Collider2D collider)
	{
		// This actually doesn't work... if one collider leaves and another is
		//  still active, we shouldn't unblock movement
		if(Utilities.ShouldColliderBlock(collider)) {
			BlockMovement (direction, false);
		}
		activeCollisions.Remove (collider);
	}
	
	// Update is called once per frame
	void Update () {
		var newAnimation = string.Empty;
		var positionChangeX = 0f;
		var positionChangeY = 0f;

		if (Input.GetKey(KeyCode.RightArrow)) {
			newAnimation = "Char-Scott_walk_right";
			if(!moveBlocked_right) {
				positionChangeX += movespeed;
			}
		} 
		if (Input.GetKey (KeyCode.LeftArrow)) {
			newAnimation = "Char-Scott_walk_left";
			if(!moveBlocked_left) {
				positionChangeX -= movespeed;
			}
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			newAnimation = "Char-Scott_walk_down";
			if(!moveBlocked_down) {
				positionChangeY -= movespeed;
			}
		} 
		if (Input.GetKey (KeyCode.UpArrow)) {
			newAnimation = "Char-Scott_walk_up";
			if(!moveBlocked_up) {
				positionChangeY += movespeed;
			}
		}

		// Set the new position
		if (positionChangeX != 0 || positionChangeY != 0) {
			transform.position = new Vector3 (transform.position.x + positionChangeX, transform.position.y + positionChangeY);
		}

		// Start the new animation
		if (newAnimation != string.Empty) {
			anim.enabled = true;
			anim.Play (newAnimation);
		}

		if (Input.anyKey == false) {
			anim.enabled = false;
		}
	}
}
