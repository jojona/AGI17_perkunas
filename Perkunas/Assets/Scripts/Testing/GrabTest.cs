using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTest : MonoBehaviour {

	public Vector3 height;
	bool hasGrabed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasGrabed) {
			GetComponent<Grabber>().grab ();
			hasGrabed = true;
		}
		transform.Translate(height);
	
	}
}
