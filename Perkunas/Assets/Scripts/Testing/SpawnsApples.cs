using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnsApples : MonoBehaviour
{

    public Object ApplePrefab;
    Quaternion quat = new Quaternion();
    public bool done = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > 3 && !done || Input.GetKeyDown("q"))
        {
            System.Random rnd = new System.Random();
            int x = rnd.Next(-5,5);
            int z = rnd.Next(-5,5);
            Instantiate(ApplePrefab, this.transform.position + new Vector3(x, 0.125f, z), quat);
            done = true;
        }

        if (Input.GetKeyDown("s"))
        {
            GameObject apple = GameObject.FindGameObjectWithTag("Apple");
            Destroy(apple);
        }
    }
}
