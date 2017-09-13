using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour {
	//information of what we will grab next
	LinkedList<Collision> colliders = new LinkedList<Collision>();
	GameObject grabbed = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	//these two functions track what objects are currently colliding with the grabber
	void OnCollisionEnter (Collision col)
	{
		colliders.AddFirst (col);
	}

	void OnCollisionExit (Collision col) {
		colliders.Remove (col);
	}

	public void grab() {
		if (colliders.Count > 0) {
			Grabable g = colliders.First.Value.gameObject.GetComponent<Grabable> ();
				if(g != null) {
					grabbed = colliders.First.Value.gameObject;
					g.attach(this.gameObject);
				}
		}
	}

	public void ungrab() {
		grabbed.GetComponent<Grabable>().detach ();
		grabbed = null;
	}
}
