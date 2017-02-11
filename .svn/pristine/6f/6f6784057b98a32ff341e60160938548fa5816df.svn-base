using UnityEngine;
using System.Collections;

public class EnemyCollisionCheck : MonoBehaviour {
	
	public EnemyBase enemy;
	public string direction;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		enemy.CollisionEvent (direction, collider, true);
	}
	
	void OnTriggerExit2D(Collider2D collider)
	{
		enemy.CollisionEvent (direction, collider, false);
	}
	
}
