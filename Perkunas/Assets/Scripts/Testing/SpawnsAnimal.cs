using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnsAnimal : MonoBehaviour {

    public Object AnimalPrefab;
    Quaternion quat = new Quaternion();

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("enter"))
        {
            System.Random rnd = new System.Random();
            int x;
            int z;
            if (rnd.Next(0, 2) == 0)
                x = -1;
            else
                x = 1;
            if (rnd.Next(0, 2) == 0)
                z = -1;
            else
                z = 1;
            float y = 0.25f;

            Instantiate(AnimalPrefab, this.transform.position + new Vector3(x,y,z), quat);
        }
    }
}
