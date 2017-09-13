using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowTree : MonoBehaviour {
	public float speed;
	public float maxHigh;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (transform.localScale.y < maxHigh){
			transform.localScale += new Vector3(0, speed, 0);
			transform.Translate(new Vector3(0, speed/2f, 0));
		}
	}

	//void OnTriggerEnter(Collider other) {
	//	if (other.gameObject.CompareTag("RainDrop")){
	//		other.gameObject.SetActive(false);
	//		if (transform.localScale.y < maxHigh){
	//			transform.localScale += new Vector3(0, speed, 0);
	//			transform.Translate(new Vector3(0, 0.1f, 0));
	//		}
	//	}
	//}
}