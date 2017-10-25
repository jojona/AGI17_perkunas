using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayBeforeGrowth : MonoBehaviour {
	Animator anim;
	System.DateTime startTime;
	float time = 0;
	float delay;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		// delay = Random.Range (3, 10);
		delay = 0; //Random.Range (3.0f, 10.0f);
		// startTime = System.DateTime.UtcNow;


	}
	
	// Update is called once per frame
	void Update () {
		// System.DateTime delta = System.DateTime.UtcNow - startTime;
		time += Time.deltaTime;

		if (time > delay) {

			anim.SetFloat ("mySpeed", 10);
		}
	}
}