using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBolt : MonoBehaviour {

    public GameObject boltPrefab;
    
	void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Cloud")
        {
            Vector3 vec = new Vector3(0, 0, 0);
            Quaternion quat = new Quaternion();
            var instance = Instantiate(boltPrefab, transform.position, quat);
            LightningBoltScript script = instance.GetComponent<LightningBoltScript>();
            Vector3 down = transform.position;
            down.y = 0;
            script.StartPosition = transform.position;
            script.EndPosition = down;
        }
    }
}
