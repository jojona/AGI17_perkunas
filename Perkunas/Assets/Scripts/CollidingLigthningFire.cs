using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingLigthningFire : MonoBehaviour {


	void Start() {
		GetComponent<BoxCollider> ().center = new Vector3 (0,-transform.position.y , 0); 
	}

	void Update(){
		GetComponent<BoxCollider> ().center = new Vector3 (0,-transform.position.y , 0); 
	}
	// Update is called once per frame
	void OnTriggerEnter(Collider other){
		Debug.Log (gameObject.name + " is trigged by  " + other.gameObject.name);		
	}
}
