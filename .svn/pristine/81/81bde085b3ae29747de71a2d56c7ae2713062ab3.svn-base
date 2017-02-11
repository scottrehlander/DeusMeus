using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	float movespeed = .01f;

	public string state = "walking";
	public string animationPrefix = "";

	float timeSpentInDirection = 0;
	float timeAllowedToRoam = 5f;

	float timeSpentDwelling = 0;
	float timeAllowedToDwell = 2;

	public float direction = 1;

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

		Turn ();
		//anim.Play (animationPrefix + "_walk_right");
	}

	void OnTriggerEnter2D(Collider2D other) {
		//state = "stopped";
		//if (!(other.transform is Hero)) {
			// If the NPC collides with something that isn't the hero, 
			//  stop and dwell for a second before turning.
			state = "dwelling";
		//}
	}

	void OnTriggerExit2D(Collider2D other) {
		state = "walking";
	}

	// Update is called once per frame
	void Update () {
		if (state.Equals ("walking")) {
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
		// Dwell and walk for a random amount of time
		timeAllowedToRoam = Random.Range (3, 12);
		timeAllowedToDwell = Random.Range (1, 3);

		direction *= -1;
		if (direction > 0) {
			anim.Play (animationPrefix + "right");
		}
		else {
			anim.Play (animationPrefix + "left");
		}
		timeSpentInDirection = 0;
	}
}
