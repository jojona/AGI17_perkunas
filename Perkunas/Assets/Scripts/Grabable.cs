using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour {
	private GameObject attachedTo = null;//this is an issue. We need to recognise multiple grabbers.
		

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}
	public virtual void attach(GameObject g) {
		FixedJoint fx = g.gameObject.AddComponent<FixedJoint>();
    		fx.breakForce = 20000;
    		fx.breakTorque = 20000;

		fx.connectedBody = GetComponent<Rigidbody>();
		attachedTo = g;
	}

	public virtual void detach(Vector3 vel, Vector3 ang) {
		if (attachedTo != null && attachedTo.GetComponent<FixedJoint>() != null)
			{
			// 2
			attachedTo.GetComponent<FixedJoint>().connectedBody = null;
			Destroy(attachedTo.GetComponent<FixedJoint>());
			// 3
			GetComponent<Rigidbody>().velocity = vel;
			GetComponent<Rigidbody>().angularVelocity = ang;
			}		

		attachedTo = null;
		
	}
}
