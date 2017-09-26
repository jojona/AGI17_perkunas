using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMoves : MonoBehaviour {

    public Rigidbody rb;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, -5);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Time.time >= 5)
            rb.velocity = new Vector3(0, 0, 0);
        if (Time.time >= 7)
            rb.velocity = new Vector3(5, 0, 0);
    }
}
