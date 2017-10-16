using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour {

	public GameObject apple;

	private float time;
	private float spawnTime = 120;

	// Use this for initialization
	void Start () {
		time = Time.time;
		spawnTime = Random.Range (1, 2);
	}
	
	// Update is called once per frame
	void Update () {
		
		if( this.tag == "Grown") {
			if (Time.time - time > spawnTime) {
				time = Time.time;
				spawnTime = Random.Range (1, 2);
				Debug.Log ("Spawning Apple");
				Instantiate (apple, transform.position + new Vector3 (Random.Range (-1f, 1f), 3f, Random.Range (-1f, 1f)), apple.transform.rotation);
			}
		}

	}
}
