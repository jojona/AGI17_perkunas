using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableTerrain : Grabable {
	GameObject manipulator = null;

	public float effectRadius = 1.0f;
	private float resolutionPerUnit;
	public Terrain terrain;
	// Use this for initialization
	void Start () {
		if (terrain == null) {
			terrain = GetComponent<Terrain> ();
		}
		if (terrain == null) {
			Debug.Log ("no terrain in GrabableTerrain");
			return;
		}
		resolutionPerUnit = ((float)terrain.terrainData.heightmapResolution) / 10.0f;//((float)terrain.terrainData.heightmapWidth);
	}


	private float max(float a, float b) {
		return a > b ? a : b;
	}
	private float min(float a, float b) {
		return a < b ? a : b;
	}
	// Update is called once per frame
	void Update () {
		if (manipulator != null && terrain != null) {
			Vector2 manipRelPos = new Vector2 (manipulator.transform.position.x - transform.position.x, manipulator.transform.position.z- transform.position.z);
			float manipRelHeight = manipulator.transform.position.y - transform.position.y;
			int absX = (int)(manipRelPos.x * resolutionPerUnit);
			int absY = (int)(manipRelPos.y * resolutionPerUnit);
			float newHeight = manipRelHeight / 3;
			newHeight = newHeight > 1 ? 1 : newHeight;
			newHeight = newHeight < 0 ? 0 : newHeight;
			int cubeWidth = (int)(effectRadius * resolutionPerUnit);
			//Debug.Log ("relative position: "+ manipRelPos + ", relative height: " + manipRelHeight + ", relative height abs: " + relHeightAbs);
			//Debug.Log ("absX: "+ absX + ", absY: " + absY);
			//Debug.Log ("resu: "+ resolutionPerUnit);
			int maxDist = cubeWidth/2;
			float offset = (float)0.1 / 600;
			if(newHeight - offset > terrain.terrainData.GetHeight(absX - maxDist, absY - maxDist)) {
				newHeight -= offset;
				float[,] data = terrain.terrainData.GetHeights (absX - maxDist, absY - maxDist, cubeWidth, cubeWidth);
				Debug.Log("terrain height1: " + data[maxDist,maxDist]);
				for (int x = 0; x < cubeWidth; ++x) {
					for (int y = 0; y < cubeWidth; ++y) {
						float dist = (x - (maxDist))*(x-(maxDist));
						dist += (y - (maxDist)) * (y - (maxDist));
						dist = Mathf.Sqrt (dist);
						data [x, y] = max(newHeight * (float)Mathf.Cos ((float)((dist / (float)maxDist) * 0.5 * Mathf.PI)), data [x, y]);
						data [x, y] = data [x, y] < 0.0f ? 0.0f : data [x, y];
						data [x, y] = data [x, y] > 1.0f ? 1.0f : data [x, y];
					}
				}
				Debug.Log ("set to: " + data [maxDist, maxDist]);

				terrain.terrainData.SetHeights(absX - maxDist, absY - maxDist, data);
			} else if(newHeight + offset < terrain.terrainData.GetHeight(absX - maxDist, absY - maxDist)){
				newHeight += offset;
				float[,] data = terrain.terrainData.GetHeights (absX - maxDist, absY - maxDist, cubeWidth, cubeWidth);
				Debug.Log("terrain height2: " + data[0,0]);
				for (int x = 0; x < cubeWidth; ++x) {
					for (int y = 0; y < cubeWidth; ++y) {
						float dist = (x - (maxDist))*(x-(maxDist));
						dist += (y - (maxDist)) * (y - (maxDist));
						dist = Mathf.Sqrt (dist);
						data [x, y] = min((1.0f-newHeight) * (float)Mathf.Cos ((float)((dist / (float)maxDist) * 0.5 * Mathf.PI)), data [x, y]);
						data [x, y] = data [x, y] < 0.0f ? 0.0f : data [x, y];
						data [x, y] = data [x, y] > 1.0f ? 1.0f : data [x, y];
					}
				}
				Debug.Log ("set to: " + data [0, 0]);

				terrain.terrainData.SetHeights(absX - maxDist, absY - maxDist, data);
			}
		}
	}

	public override void attach(GameObject g) {
		manipulator = g;
	}

	public override void detach(Vector3 a, Vector3 b) {
		manipulator = null;
		//we do nothing special with the release speeds here
	}
}
