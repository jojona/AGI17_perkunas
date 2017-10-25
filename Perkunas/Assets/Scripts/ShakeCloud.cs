using UnityEngine;
using System;

public class ShakeCloud : MonoBehaviour {

	public bool startWithRain = false;

    private bool isRaining = false;
	private bool isShaken = false;

    private Rigidbody rb;
    private ParticleSystem rain;
    private float timer;
    private float nextActionTime = 2;
    private float period = 1;
    private GameObject[] trees;
    public Renderer rend;
    private float shakeMagnitudeMax = 1000;
    private float shakeMagnitudeMin = 10;
    private Vector3 actualVelocity;
    private Vector3 previousPosition;

    void Start()
    {
        timer = -1; //so we can shake right away
        rb = GetComponent<Rigidbody>();
        previousPosition = rb.position;
        rain = GetComponentInChildren<ParticleSystem>();
		rend = GetComponent<Renderer>();

		if (!startWithRain) {
			rain.Stop ();
		} else {
			rend.material.color = Color.gray;
			isRaining = true;
		}
        
        
    }

    void GrowATree()
    {
        Vector3 vec = new Vector3(rb.position.x, 1, rb.position.z);
        Quaternion quat = new Quaternion();

        Instantiate(GameObject.Find("Tree"), vec, quat);
    }

    // Update is called once per frame
    void Update () {
        //TODO : Only check when it's grabbed
        actualVelocity = (rb.position - previousPosition)/Time.deltaTime;
		if (actualVelocity.magnitude >= shakeMagnitudeMin && actualVelocity.magnitude < shakeMagnitudeMax  && !isShaken && (Time.time - timer >= 1))
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
        if (actualVelocity.magnitude < shakeMagnitudeMin && (Time.time - timer >= 1))
            isShaken = false;

        if (isRaining)
        {
            trees = GameObject.FindGameObjectsWithTag("Seed");
            foreach (GameObject obj in trees)
            {
                if (Math.Abs(obj.transform.position.x - transform.position.x) < 2 && Math.Abs(obj.transform.position.z - transform.position.z) < 2)
                {
                    Animator ani = obj.GetComponent<Animator>();
                    ani.SetFloat("mySpeed", 0.7f);
                    obj.tag = "Grown";
					obj.GetComponent<setOnFire> ().changeFire();
                }
            }
        }
        previousPosition = rb.position;
    }
}
