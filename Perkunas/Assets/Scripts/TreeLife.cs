using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLife : MonoBehaviour {

	float timeWithRain = 0.0f;
	const float timeLimit = 20.0f;	

	Color[] initialColor;

	bool dry = false;
	bool wet = false;
	bool rain = false;
	bool dead = false;

	public void raining() {
		rain = true;
	}

	// Use this for initialization
	void Start () {
		GameObject model = transform.GetChild (1).gameObject;
		Renderer rend = model.GetComponent<Renderer>();
		Material[] mats = rend.materials;
		initialColor = new Color[mats.Length];
		for(int i = 0; i < mats.Length; i++) {
			initialColor[i] = mats [i].color;
			Debug.Log ("\"" + mats [i].name + "\"");
		}

	}
	
	bool a = true;

	// Update is called once per frame
	void Update () {
		float t = Time.deltaTime;
		if (rain) {
			timeWithRain += t;
		} else {
			timeWithRain -= t;
		}
			


		GameObject model = transform.GetChild (1).gameObject;
		Renderer rend = model.GetComponent<Renderer>();
		if (Mathf.Abs (timeWithRain) > timeLimit * 2.0f) {
			// TODO death state
			dead = true;
			Material[] mats = rend.materials;
				for(int i = 0; i < mats.Length; i++) {
				if (mats [i].name != "Bark (Instance)") {
					mats [i].SetColor ("_Color", Color.red);
				}
			}
		}
		if (Mathf.Abs (timeWithRain) > timeLimit * 4.0f) {
			// Die
			Destroy(this.gameObject);
		}

		if (!dead) {
			if (!wet && timeWithRain > timeLimit) {
				Material[] mats = rend.materials;
				for(int i = 0; i < mats.Length; i++) {
					if (mats [i].name != "Bark (Instance)") {
						mats [i].SetColor ("_Color", new Color(105f/255f, 51f/255f, 0f));
					}
				}
				wet = true;
			} else if (!dry && timeWithRain < -timeLimit) {
				Material[] mats = rend.materials;
				for(int i = 0; i < mats.Length; i++) {
					if (mats [i].name != "Bark (Instance)") {
						mats [i].SetColor ("_Color", Color.yellow);
					}
				}
				dry = true;
			} else if (wet && !rain ){//timeWithRain < timeLimit) {
				timeWithRain = timeLimit-0.5f;
				Material[] mats = rend.materials;
				for(int i = 0; i < mats.Length; i++) {
					if (mats [i].name != "Bark (Instance)") {
						mats [i].SetColor ("_Color", initialColor [i]);
					}
				}
				wet = false;
			} else if (dry && rain){//timeWithRain > -timeLimit) {
				timeWithRain = -timeLimit+0.5f;
				Material[] mats = rend.materials;
				for(int i = 0; i < mats.Length; i++) {
					if (mats [i].name != "Bark (Instance)") {
						mats[i].SetColor("_Color", initialColor[i]);
					}
				}
				dry = false;
			}
		}
		rain = false;
	}
}



