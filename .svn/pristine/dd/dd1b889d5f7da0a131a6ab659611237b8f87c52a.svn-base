using UnityEngine;
using System.Collections;

public class HeroIsInSight : MonoBehaviour {

	public Hero hero;
	public EnemyBase enemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.transform.parent == hero.transform) {
			enemy.VisionEvent(true);
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.transform.parent == hero.transform) {
			enemy.VisionEvent(false);
		}
	}
}
