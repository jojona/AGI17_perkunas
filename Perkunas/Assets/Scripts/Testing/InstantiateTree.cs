using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTree : MonoBehaviour {

    public Object Tree;

    // Update is called once per frame
    void Update () {
        
        if (Input.GetMouseButtonDown(0))
        {
            System.Random rnd = new System.Random();
            int x = rnd.Next(-5, 5);  
            int z = rnd.Next(-5, 5);
            Vector3 vec = new Vector3(x,1,z);
            Quaternion quat = new Quaternion();

            Instantiate(Tree, vec, quat);
        }
    }
}
