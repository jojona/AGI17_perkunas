using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTowardsAltitude : MonoBehaviour {
	public float height = 0.0f;
	public float forcemultiplier = 1.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Rigidbody r = GetComponent<Rigidbody> ();
		if (r != null) {
			//create a force up or down depending on our relative altitude to the terget altitude.
			r.AddForce (new Vector3 (0, 1, 0) * (height - transform.position.y) * forcemultiplier);
		}
	}
}
