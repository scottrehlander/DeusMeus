using UnityEngine;
using System.Collections;

public class ShowSpawnPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawCube(transform.position, new Vector3(.1F, .1F, .1F));
#if UNITY_EDITOR
		UnityEditor.Handles.Label(transform.position, transform.name);
#endif
	}
}

