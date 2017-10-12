using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehavior : MonoBehaviour {

    public float speed = 0.05f;
    public float directionChangeInterval = 1;
    public float maxHeadingChange = 30;
    
    float heading;
    Vector3 targetRotation;
    Rigidbody rb;
    CharacterController cc;
    float sleepTime;
    bool asleep = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();

        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 90);
    }

    void Update()
    {
        if (asleep)
        {
            sleepTime -= Time.deltaTime;
            if (sleepTime < 0)
                asleep = false;
        }

        else
        {
            float odd = Mathf.Min(0.10f / (1.0f / (Time.deltaTime)), 1);
            float rand = Random.Range(0.0f, 1.0f);

            Debug.Log(odd);
            Debug.Log(rand);
            if (rand < odd)
            {
                asleep = true;
                sleepTime = Random.Range(2f, 7f);
            }
            var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
            var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
            heading = Random.Range(floor, ceil);
            targetRotation = new Vector3(0, heading, 90);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
            var forward = transform.TransformDirection(new Vector3(0, 1, 0));
            cc.SimpleMove(speed * forward);
        }
    }
}
