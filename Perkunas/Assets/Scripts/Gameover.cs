using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class will spawn the gameover text. 
// This also contains keyboard input to increase the gametime or restart the scene
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

        // Restart the scene
		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		// Give the player more time
		if (Input.GetKeyDown(KeyCode.T)) {
			canvas.SetActive (false);
			startTime += 60;
		}

		// Give the player less time
		if (Input.GetKeyDown(KeyCode.Y)) {
			canvas.SetActive (false);
			startTime -= 60;
		}
	}
}
