using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour {
	public GameObject attachedTo = null;
		

	// Use this for initialization
	void Start () {
		if (attachedTo != null) {
			this.gameObject.transform.parent = attachedTo.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void attach(GameObject g, SteamVR_Controller.Device Controller) {
		FixedJoint fx = g.gameObject.AddComponent<FixedJoint>();
    		fx.breakForce = 20000;
    		fx.breakTorque = 20000;

		fx.connectedBody = GetComponent<Rigidbody>();
		attachedTo = g;
	}

	public void detach(Vector3 vel, Vector3 ang) {
		if (attachedTo != null && attachedTo.GetComponent<FixedJoint>())
			{
			// 2
			attachedTo.GetComponent<FixedJoint>().connectedBody = null;
			attachedTo.Destroy(GetComponent<FixedJoint>());
			// 3
			GetComponent<Rigidbody>().velocity = vel;
			GetComponent<Rigidbody>().angularVelocity = ang;
			}		

		attachedTo = null;
		
	}
}
