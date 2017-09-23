using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCloud : MonoBehaviour {

    private bool isRaining = false;
    private bool isShaken = false;

    public Rigidbody rb;
    public ParticleSystem rain;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rain = GetComponentInChildren<ParticleSystem>();
        rain.Stop();
    }
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.magnitude >= 10 && !isShaken)
        {
            isShaken = true;
            isRaining = !isRaining;
            if (isRaining)
                rain.Play();
            else
                rain.Stop();
        }
        if (rb.velocity.magnitude < 10)
            isShaken = false;

    }
}
