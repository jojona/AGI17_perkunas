using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will kill objects that have fallen of the map. 
public class Death : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -1.0f) {
			Destroy (this.gameObject);
		}
	}		
}
