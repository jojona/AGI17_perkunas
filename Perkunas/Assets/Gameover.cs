using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameover : MonoBehaviour {

	public GameObject canvas;

	float startTime;

	// Use this for initialization
	void Start () {
		canvas.SetActive (false);
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time - startTime > 120) {
			canvas.SetActive (true);
		}


	}
}
