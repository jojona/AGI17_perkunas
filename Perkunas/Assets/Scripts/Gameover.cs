using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour {

	public GameObject canvas;

	float startTime;

    private bool printBool = true;

	// Use this for initialization
	void Start () {
		canvas.SetActive (false);
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time - startTime > 120) {
			canvas.SetActive (true);
		}

        if (printBool && ((int)Time.time - (int)startTime) % 5 == 0)
        {
            Debug.Log(Time.time - startTime);
            printBool = false;
        }
        if (((int)Time.time - (int)startTime) % 5 == 2)
        {
            printBool = true;
        }

		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		if (Input.GetKeyDown(KeyCode.T)) {
			canvas.SetActive (false);
			startTime += 60;
		}
		if (Input.GetKeyDown(KeyCode.Y)) {
			canvas.SetActive (false);
			startTime -= 60;
		}
	}
}
