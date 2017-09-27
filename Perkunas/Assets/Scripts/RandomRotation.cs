using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour {

    // Use this for initialization
    void Start () {
		float angle = Random.Range (0, 360);
		transform.Rotate (0, angle, 0);
    }
	
	// Update is called once per frame
	void Update () {
    }

}
