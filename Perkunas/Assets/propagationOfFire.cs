using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propagationOfFire : MonoBehaviour {
	List<Collider> TriggerList;

	public float timeLimit = 3.0f;
	private float lifetime = 0.0f;
	private bool willPropag = false;

	void Start(){
		TriggerList = new List<Collider> ();
	}

	void OnTriggerEnter(Collider other)
	{
		//if the object is not already in the list
		if(!TriggerList.Contains(other))
		{
			//add the object to the list
			TriggerList.Add(other);
			//Debug.Log ("Add to the list, the object : " + other.gameObject.transform.parent.gameObject.name);
		}
	}

	void OnTriggerExit(Collider other)
	{
		//if the object is in the list
		if(TriggerList.Contains(other))
		{
			//remove it from the list
			TriggerList.Remove(other);
			//Debug.Log ("Remove from the list, the object : " + other.gameObject.transform.parent.gameObject.name);
		}
	}

	public void propagationFire(){
		foreach (Collider other in TriggerList){
			other.gameObject.GetComponent<propagationOfFire> ().prepareFire();
		}
	}

	public void Update(){
		lifetime += Time.deltaTime;
		if (willPropag && lifetime > timeLimit) {
			willPropag = false;
			gameObject.transform.transform.parent.gameObject.GetComponent<setOnFire> ().StartFire ();
		}
	}

	public void prepareFire(){
		if (!willPropag) {
			willPropag = true;
			lifetime = 0.0f;
		}
	}

}
