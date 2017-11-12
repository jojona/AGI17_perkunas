using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour {	

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}
	//attach object to controller
	public virtual void attach(GameObject g) {
		FixedJoint fx = g.gameObject.AddComponent<FixedJoint>();
    		fx.breakForce = 2000000;
    		fx.breakTorque = 2000000;

		fx.connectedBody = GetComponent<Rigidbody>();
	}

	//detach object from controller
	public virtual void detach(GameObject g, Vector3 vel, Vector3 ang) {
		if (g != null && g.GetComponent<FixedJoint>() != null)
			{
			// 2
			g.GetComponent<FixedJoint>().connectedBody = null;
			Destroy(g.GetComponent<FixedJoint>());
			// 3
			GetComponent<Rigidbody>().velocity = vel;
			GetComponent<Rigidbody>().angularVelocity = ang;
			}		
	}
}
