using UnityEngine;
using System.Collections;

public class TransitionScene : MonoBehaviour {

	public Transform hero;
	public string destinationScene;
	public string destinationSpawnPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter2D(Collider2D collider) {
		if (collider.transform.parent == hero) {
			PlayerPrefs.SetString("SpawnPosition", destinationSpawnPoint);
			Application.LoadLevel(destinationScene);
		}
	}

}
