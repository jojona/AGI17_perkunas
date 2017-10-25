using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehavior : MonoBehaviour {
    
    public float speed = 0f;
    public float maxSpeed = 2f;
    public float acceleratingTime = 0.5f;
    public float zRotation = 0;

    public float directionChangeInterval = 1;
    public float maxHeadingChange = 10;

    public float lifeTime = 30;
    public float fadingSpeed = 3;

    public bool isDead = false;
    public float timeUntilFalling = 7;
    
    float heading;

    private GameObject apple;
    private bool foundApple;
    private float appleLifeTime = 4.958f;

    Vector3 targetRotation;
    CharacterController cc;
    Animator anim;

    public Renderer rend;
    float sleepTime;
    bool asleep = false;
    float spawningTime;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, zRotation);
        spawningTime = Time.time;
        rend = GetComponent<Renderer>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (isDead)
        {
            if (timeUntilFalling > 0)
                timeUntilFalling = timeUntilFalling - Time.deltaTime;
            else {
                Vector3 offset = new Vector3(0, 0.01f, 0);
                transform.position = transform.position - offset;
                if (transform.position.y < -3)
                    Destroy(gameObject);
            }
            return;
        }
            

        if (Time.time - spawningTime > lifeTime)
        {
            isDead = true;
            anim.SetInteger("Behaviour", 2);
        }

        if (asleep)
        {
            sleepTime -= Time.deltaTime;
            if (sleepTime < 0)
            {
                asleep = false;
                anim.SetInteger("Behaviour", 0);
            }
        }

        else
        {
            if (!foundApple)
                apple = GameObject.FindGameObjectWithTag("Apple");
            if (apple != null)
            {
                foundApple = true;
            }

            if (!foundApple)
            {
                float odd = Mathf.Min(0.25f / (1.0f / (Time.deltaTime)), 1);
                float rand = Random.Range(0.0f, 1.0f);

                if (rand < odd)
                {
                    asleep = true;
                    speed = 0;
                    anim.SetInteger("Behaviour", 1);
                    sleepTime = 4.958f;
                }

                var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
                var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
                heading = Random.Range(floor, ceil);
                targetRotation = new Vector3(0, heading, zRotation);
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
                var forward = transform.TransformDirection(new Vector3(1, 0, 0));

                if (speed < maxSpeed)
                {
                    speed += maxSpeed * (Time.deltaTime / acceleratingTime);
                }
                cc.SimpleMove(speed * forward);
            }
            else
            {
                if (apple == null)
                {
                    foundApple = false;
                    heading = Random.Range(0, 360);
                    transform.eulerAngles = new Vector3(0, heading, zRotation);
                    anim.SetInteger("Behaviour", 0);
                    return;
                }
                    
                transform.LookAt(apple.transform);
                transform.eulerAngles += new Vector3(0, -90, 0);
                
                if (Vector3.Distance(transform.position, apple.transform.position) < 0.7)
                {
                    anim.SetInteger("Behaviour", 1);
                    appleLifeTime -= Time.deltaTime;
                    if (appleLifeTime < 0)
                    {
                        appleLifeTime = 4.958f;
                        Destroy(apple);
                        foundApple = false;
                        heading = Random.Range(0, 360);
                        transform.eulerAngles = new Vector3(0, heading, zRotation);
                        anim.SetInteger("Behaviour", 0);
                    }
                }
                else
                {
                    if (speed < maxSpeed)
                    {
                        speed += maxSpeed * (Time.deltaTime / acceleratingTime);
                    }
                    var forward = transform.TransformDirection(new Vector3(1, 0, 0));
                    cc.SimpleMove(speed * forward);
                }

            }
        }
    }
}
