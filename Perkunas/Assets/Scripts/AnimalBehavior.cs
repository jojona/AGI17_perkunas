using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehavior : MonoBehaviour {
    
    public float speed = 0f;
    public float maxSpeed = 2f;
    public float acceleratingTime = 0.3f;

    public float directionChangeInterval = 1;
    public float maxHeadingChange = 30;

    public float lifeTime = 30;
    public float fadingSpeed = 3;
    
    float heading;
    Vector3 targetRotation;
    Rigidbody rb;
    CharacterController cc;
    public Renderer rend;
    float sleepTime;
    bool asleep = false;
    float spawningTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();

        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 90);
        spawningTime = Time.time;
        rend = GetComponent<Renderer>();
        //TODO : start animation walk
    }

    void Update()
    {
        if (Time.time - spawningTime > lifeTime)
        {
            Color col = rend.material.color;
            col[3] = col[3] - Time.deltaTime / fadingSpeed;
            if (col[3] < 0)
            {
                Destroy(gameObject);
                return;
            }

            rend.material.SetColor("_Color", col);
        }

        if (asleep)
        {
            sleepTime -= Time.deltaTime;
            if (sleepTime < 0)
            {
                asleep = false;
                //TODO : start animation walking
            }
        }

        else
        {
            float odd = Mathf.Min(0.25f / (1.0f / (Time.deltaTime)), 1);
            float rand = Random.Range(0.0f, 1.0f);
            
            if (rand < odd)
            {
                asleep = true;
                speed = 0;
                /*TODO : int animation = Random.Range(0, 2);
                if (animation == 0)
                    Start animation eating
                else
                    Start animation looking around*/
                sleepTime = Random.Range(2f, 5f);
            }
            var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
            var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
            heading = Random.Range(floor, ceil);
            targetRotation = new Vector3(0, heading, 90);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
            var forward = transform.TransformDirection(new Vector3(0, 1, 0));

            if (speed < maxSpeed)
            {
                speed += maxSpeed*(Time.deltaTime/acceleratingTime);
            }


            cc.SimpleMove(speed * forward);
        }
    }
}
