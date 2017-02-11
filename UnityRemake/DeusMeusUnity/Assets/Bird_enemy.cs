using UnityEngine;
using System.Collections;

public class Bird_enemy : EnemyBase {

	public Transform hero;
	bool heroInSight = false;

	public AudioClip squawkSound;

	enum BirdState 
	{
		Roaming,
		Attacking
	};
	private BirdState birdState;

	// Use this for initialization
	void Start () {
		birdState = BirdState.Roaming;
		Setup ();
	}

	float timeToDelayUntilAttack = .2F;
	float timeToAttackFor = .8F;
	float timeDelayed = 0;
	float timeAttacking = 0;
	float attackSpeedDelta = .05F;
	Vector2 attackDirection = Vector2.zero;
	void Update () {
		if (heroInSight || birdState == BirdState.Attacking) {
			birdState = BirdState.Attacking;

			// If move is blocked while attacking, just stop
			if(moveBlocked_up || moveBlocked_right || moveBlocked_left || moveBlocked_down ) {
				// Do nothing
				return;
			}

			// Delay for a bit before attacking
			if (timeDelayed <= timeToDelayUntilAttack) {
					timeDelayed += Time.deltaTime;
			} 
			else if (attackDirection == Vector2.zero) {
				// Pick the attack direction
				var xDiff = hero.position.x - transform.position.x;
				var yDiff = hero.position.y - transform.position.y;

				// Find the hypotenuse
				// x^2 + y^2 = z^2
				// 
				var hyp = Mathf.Sqrt ((Mathf.Pow (xDiff, 2) + Mathf.Pow (yDiff, 2)));
				xDiff = (float)(xDiff / hyp);
				yDiff = (float)(yDiff / hyp);
				attackDirection = new Vector2 (xDiff, yDiff);

				AudioSource.PlayClipAtPoint(squawkSound, transform.position);
			}

			if (timeDelayed > timeToDelayUntilAttack) {
				timeAttacking += Time.deltaTime;

				if(attackDirection.x > 0) {
					anim.enabled = true;
					anim.Play(animationPrefix + "right");
				}
				else {
					anim.enabled = true;
					anim.Play(animationPrefix + "left");
				}

				// Attack the hero swiftly
				transform.position = new Vector3 (transform.position.x + (attackDirection.x * attackSpeedDelta), 
	                          transform.position.y + (attackDirection.y * attackSpeedDelta),
	                          transform.position.z);


				if (timeAttacking > timeToAttackFor) {
					timeDelayed = 0;
					timeAttacking = 0;
					attackDirection = Vector2.zero;

					birdState = BirdState.Roaming;
				}
			}
		} 
		else {
			// The bird should be roaming
			birdState = BirdState.Roaming;
		}

		if (birdState == BirdState.Roaming) {
			Roam();
		}
	}

	public override void VisionEvent (bool canSee)
	{
		heroInSight = canSee;
	}

	public override void CollisionEvent (string direction, Collider2D collider, bool collisionEntered)
	{
		Debug.Log ("Collision detected in direction " + direction);
		base.CollisionEvent (direction, collider, collisionEntered);
	}

	// Enemies need to be smart, need to bake in the ability for 4 colliders (one for each direction).
	//  They should trigger a CollisionEvent(direction)
}
