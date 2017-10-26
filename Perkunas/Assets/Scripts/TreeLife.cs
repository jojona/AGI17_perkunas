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
	bool fire = false;

	public GameObject apple;

	private float time;
	private float spawnTime = 120;
	private bool destruct = false;
	float timeWithFire = 0.0f;
	public void raining() {
		rain = true;
	}

	public void setFire() {
		fire = true;
		timeWithRain = 0; // TODO timeWithFire
		timeWithFire = 0;
	}

	// Use this for initialization
	void Start () {
		GameObject model = transform.GetChild (2).gameObject;
		Renderer rend = model.GetComponent<Renderer>();
		Material[] mats = rend.materials;
		initialColor = new Color[mats.Length];
		for(int i = 0; i < mats.Length; i++) {
			initialColor[i] = mats [i].color;
			Debug.Log ("\"" + mats [i].name + "\"");
		}
		destruct = false;
		time = Time.time;
		spawnTime = Random.Range (30, 40);
	}
	
	bool a = true;

	// Update is called once per frame
	void Update () {
		float t = Time.deltaTime;
		if (fire) {
			timeWithFire += t;
		}

		// Remove fire
		if (rain && fire && timeWithRain > 5.0f) { // Fire for 5 seconds and rain
			GetComponent<setOnFire> ().removeFire();
			fire = false;
		}


		if (this.tag == "Seed") {

			if (destruct && timeWithFire > 5.0f) {
				Destroy (this.gameObject);
			}
			if (destruct) {
				return;
			}

			//destruct the seed if to long on fire
			if ((!(destruct)) && fire && timeWithFire > 4.5f) {
				GetComponent<setOnFire> ().removeFire ();
				GetComponent<setOnFire> ().setDead ();
				destruct = true;
			
			}
			rain = false;
		}


		if (this.tag != "Grown") {
			return;
		}

		if (apple != null && !dry && !wet && !dead && Time.time - time > spawnTime) {
			time = Time.time;
			spawnTime = Random.Range (30, 40);
			Debug.Log ("Spawning Apple");
			Instantiate (apple, transform.position + new Vector3 (Random.Range (-1f, 1f), 3f, Random.Range (-1f, 1f)), apple.transform.rotation);
		}

		if (rain || fire) {
			timeWithRain += t;
		} else {
			timeWithRain -= t;
		}



		GameObject model = transform.GetChild (2).gameObject;
		Renderer rend = model.GetComponent<Renderer>();
		if ((fire && timeWithRain > timeLimit) || Mathf.Abs (timeWithRain) > timeLimit * 2.0f) {
			// Remove fire
			GetComponent<setOnFire>().setDead();
			GetComponent<setOnFire> ().removeFire();
			fire = false;

			// Die
			dead = true;
			Material[] mats = rend.materials;
				for(int i = 0; i < mats.Length; i++) {
				if (mats [i].name != "Bark (Instance)") {
					mats [i].SetColor ("_Color", Color.red);
				}
			}

			model.SetActive (false);
			transform.GetChild (4).gameObject.SetActive (true);
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



