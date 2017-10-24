using UnityEngine;
using System;

public class ShakeCloud : MonoBehaviour {

    public bool isRaining = false;
    private bool isShaken = false;

    private Rigidbody rb;
    private ParticleSystem rain;
    private float timer;
    private float nextActionTime = 2;
    private float period = 1;
    private GameObject[] trees;
    public Renderer rend;
    private float shakeMagnitude = 10;

    void Start()
    {
        timer = -1; //so we can shake right away
        rb = GetComponent<Rigidbody>();
        rain = GetComponentInChildren<ParticleSystem>();
        rain.Stop();
        rend = GetComponent<Renderer>();
    }

    void GrowATree()
    {
        Vector3 vec = new Vector3(rb.position.x, 1, rb.position.z);
        Quaternion quat = new Quaternion();

        Instantiate(GameObject.Find("Tree"), vec, quat);
    }

    // Update is called once per frame
    void Update () {
		if (rb.velocity.magnitude >= shakeMagnitude && !isShaken && (Time.time - timer >= 1))
        {
            timer = Time.time;
            isShaken = true;
            isRaining = !isRaining;
            if (isRaining)
            {
                rain.Play();
                rend.material.color = Color.gray;
            }
            else
            {
                rain.Stop();
                rend.material.color = Color.white;
            }
        }
        if (rb.velocity.magnitude < shakeMagnitude && (Time.time - timer >= 1))
            isShaken = false;

        if (isRaining)
        {
            trees = GameObject.FindGameObjectsWithTag("Seed");
            foreach (GameObject obj in trees)
            {
                if (Math.Abs(obj.transform.position.x - transform.position.x) < 2 && Math.Abs(obj.transform.position.z - transform.position.z) < 2)
                {
                    //Start Animation INSTEAD LUL 
                    obj.tag = "Grown";
                }
            }
        }
    }
}
