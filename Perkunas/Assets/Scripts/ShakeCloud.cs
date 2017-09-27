using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCloud : MonoBehaviour {

    private bool isRaining = false;
    private bool isShaken = false;

    private Rigidbody rb;
    private ParticleSystem rain;
    private float timer;
    private float nextActionTime = 2;
    private float period = 1;
    private GameObject[] trees;

    void Start()
    {
        timer = -1; //so we can shake right away
        rb = GetComponent<Rigidbody>();
        rain = GetComponentInChildren<ParticleSystem>();
        rain.Stop();
    }

    void GrowATree()
    {
        Vector3 vec = new Vector3(rb.position.x, 1, rb.position.z);
        Quaternion quat = new Quaternion();

        Instantiate(GameObject.Find("Tree"), vec, quat);
    }

    // Update is called once per frame
    void Update () {
		if (rb.velocity.magnitude >= 2 && !isShaken && (Time.time - timer >= 1))
        {
            timer = Time.time;
            isShaken = true;
            isRaining = !isRaining;
            if (isRaining)
                rain.Play();
            else
                rain.Stop();
        }
        if (rb.velocity.magnitude < 2 && (Time.time - timer >= 1))
            isShaken = false;

        if (Time.time > nextActionTime && isRaining)
        {
            trees = GameObject.FindGameObjectsWithTag("TreePrefab");
            nextActionTime += period;
            GrowATree();
        }
    }
}
