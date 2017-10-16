using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	public GameObject cloud;

	public GameObject pine;
	public GameObject leaf;

	private float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time - startTime > 10) {
			

			Vector3 vec = new Vector3(Random.Range(0, 20), 0.05f, Random.Range(0, 20));
			vec.y = 6;
			Instantiate (cloud, vec, cloud.transform.rotation);

			vec = new Vector3(Random.Range(0, 20), 0.05f, Random.Range(0, 20));
			vec.y = 6;
			Instantiate (pine, vec, pine.transform.rotation);

			vec = new Vector3(Random.Range(0, 20), 0.05f, Random.Range(0, 20));
			vec.y = 6;
			Instantiate (leaf, vec, leaf.transform.rotation);


			startTime = Time.time;
		}
	}
}
