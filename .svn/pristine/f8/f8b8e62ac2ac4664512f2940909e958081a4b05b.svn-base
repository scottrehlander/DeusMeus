using UnityEngine;
using System.Collections;

public class HeroCollisionCheck : MonoBehaviour {

	public Hero hero;
	public string direction;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		hero.CollisionDetected (direction, collider);
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		hero.CollisionClear (direction, collider);
	}

}
