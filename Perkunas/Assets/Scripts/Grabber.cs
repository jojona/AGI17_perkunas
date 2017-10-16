using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour {
	//information of what we will grab next
	LinkedList<Collider> colliders = new LinkedList<Collider>();
	GameObject grabbed = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//we may or may not need a more sophisticated way of interacting with the controllers.
		//is there any reason we may want to react to a grab by anything other than the vive controllers?
		//if(Input.
	}


	//these two functions track what objects are currently colliding with the grabber
	void OnTriggerEnter (Collider col)
	{
		colliders.AddFirst (col);
	}

	void OnTriggerExit (Collider col) {
		colliders.Remove (col);
	}

	public void grab() {
		if (grabbed == null) {
			bool noGrab = true;
			//iterate through all colliders, use the first suitable
			foreach(Collider c in colliders) {
				Grabable g = c.gameObject.GetComponent<Grabable> ();
				if (g != null) {
					grabbed = c.gameObject;
					g.attach (this.gameObject);
					Debug.Log("grabbed " + grabbed.ToString());
					noGrab = false;
					break;
				}
			}
			if(noGrab)
				Debug.Log ("nothing to grab");
		} else {
			Debug.Log("Tried to grab something while holding something");
		}
	}

	public void ungrab(Vector3 vel, Vector3 ang) {
		if (grabbed != null) {	
									Debug.Log("Ungrabbing " + grabbed.ToString());
			grabbed.GetComponent<Grabable> ().detach (this.gameObject, vel, ang);
			grabbed = null;

		} else {
			Debug.Log ("nothing to ungrab");
		}
	}
}
