using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour {
	
	float movespeed = .01f;
	
	public string state = "walking";
	public string animationPrefix = "";
	
	float timeSpentInDirection = 0;
	float timeAllowedToRoam = 5f;
	
	float timeSpentDwelling = 0;
	float timeAllowedToDwell = 2;
	
	public float direction = 1;

	protected bool moveBlocked_left = false;
	protected bool moveBlocked_right = false;
	protected bool moveBlocked_up = false;
	protected bool moveBlocked_down = false;
	protected List<Collider2D> activeCollisions;
	
	protected Animator anim;

	protected void Setup()
	{
		activeCollisions = new List<Collider2D> ();
		anim = GetComponent<Animator>();
		
		Turn ();
	}

	public virtual void VisionEvent(bool canSee)
	{
		// Do nothing
	}

	public virtual void CollisionEvent(string direction, Collider2D collider, bool collisionEntered) 
	{
		if (collisionEntered) {
			if (Utilities.ShouldColliderBlock (collider)) {
				BlockMovement (direction, true);
			}
			activeCollisions.Add (collider);
		} 
		else {
			// This actually doesn't work... if one collider leaves and another is
			//  still active, we shouldn't unblock movement
			if(Utilities.ShouldColliderBlock(collider)) {
				BlockMovement (direction, false);
			}
			activeCollisions.Remove (collider);
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
	
	protected void Roam() {
		if (state.Equals ("walking")) {

			// Make sure we didn't walk into something
			if(direction < 0 && moveBlocked_left || 
			   direction > 0 && moveBlocked_right) {
				Turn ();
				return;
			}

			if (direction > 0) {
				anim.Play (animationPrefix + "right");
			}
			else {
				anim.Play (animationPrefix + "left");
			}

			timeSpentInDirection += Time.deltaTime;
			
			var positionChangeX = movespeed * direction;
			
			transform.position = new Vector3 (transform.position.x + positionChangeX, transform.position.y);
			
			if (timeSpentInDirection > timeAllowedToRoam) {
				state = "dwelling";
			}
		}
		
		if (state.Equals ("dwelling")) {
			anim.enabled = false;
			if(timeSpentDwelling > timeAllowedToDwell) {
				state = "walking";
				anim.enabled = true;
				timeSpentDwelling = 0;
				Turn ();
			}
			timeSpentDwelling += Time.deltaTime;
		}
	}

	void Turn()
	{
		direction *= -1;

		// Dwell and walk for a random amount of time
		timeAllowedToRoam = Random.Range (3, 12);
		timeAllowedToDwell = Random.Range (1, 3);

		timeSpentInDirection = 0;
	}

}
