using UnityEngine;
using System.Collections;

public class ShowExitPoint : MonoBehaviour {

	public BoxCollider2D colliderPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawCube(transform.position, colliderPoint.size);
	}

}
