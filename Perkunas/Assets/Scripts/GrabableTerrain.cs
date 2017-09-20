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
			float newHeight = manipRelHeight / 3.0f;
			newHeight = newHeight > 1 ? 1 : newHeight;
			newHeight = newHeight < 0 ? 0 : newHeight;
			int cubeWidth = (int)(effectRadius * resolutionPerUnit);
			//Debug.Log ("relative position: "+ manipRelPos + ", relative height: " + manipRelHeight + ", relative height abs: " + relHeightAbs);
			//Debug.Log ("absX: "+ absX + ", absY: " + absY);
			//Debug.Log ("resu: "+ resolutionPerUnit);
			int maxDist = cubeWidth/2;
			float offset = (float)0.025 / 3.0f;
			if(newHeight - offset > terrain.terrainData.GetHeight(absX, absY)/3.0f) {
				newHeight -= offset;
				float[,] data = terrain.terrainData.GetHeights (absX - maxDist, absY - maxDist, cubeWidth, cubeWidth);
				//Debug.Log("terrain height1: " + data[maxDist,maxDist]);
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
				//Debug.Log ("set to: " + data [maxDist, maxDist]);

				terrain.terrainData.SetHeights(absX - maxDist, absY - maxDist, data);
			} else if(newHeight + offset < terrain.terrainData.GetHeight(absX, absY)/3.0f){
				newHeight += offset;
				float[,] data = terrain.terrainData.GetHeights (absX - maxDist, absY - maxDist, cubeWidth, cubeWidth);
				//Debug.Log("terrain height2: " + data[0,0]);
				for (int x = 0; x < cubeWidth; ++x) {
					for (int y = 0; y < cubeWidth; ++y) {
						float dist = (x - (maxDist))*(x-(maxDist));
						dist += (y - (maxDist)) * (y - (maxDist));
						dist = Mathf.Sqrt (dist);
						data [x, y] = min(1.0f - max((1.0f-newHeight) * (float)Mathf.Cos ((float)((dist / (float)maxDist) * 0.5 * Mathf.PI)),0.0f), data [x, y]);
						data [x, y] = data [x, y] < 0.0f ? 0.0f : data [x, y];
						data [x, y] = data [x, y] > 1.0f ? 1.0f : data [x, y];
					}
				}
				//Debug.Log ("set to: " + data [0, 0]);

				terrain.terrainData.SetHeights(absX - maxDist, absY - maxDist, data);
				updateTextureInSquare (absX - maxDist, absY - maxDist, maxDist);
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

	public void updateTextureInSquare(int x, int y, int width) {
		TerrainData terrData = terrain.terrainData;
		int xMax = x + width > terrData.alphamapWidth ? terrData.heightmapWidth : x + width;
		int yMax = y + width > terrData.alphamapHeight ? terrData.heightmapWidth : y + width;


		float[, ,] splatmapData = new float[xMax - x, yMax - y, terrData.alphamapLayers];
		for (int i = x; i < xMax; ++i) {
			for (int j = y; j < yMax; ++j) {
				//code from here copy-pasted from tutorial, make sure to update it as apropriate
				float y_01 = (float)y/(float)terrData.alphamapHeight;
				float x_01 = (float)x/(float)terrData.alphamapWidth;

				// Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
				float height = terrData.GetHeight(Mathf.RoundToInt(y_01 * terrData.heightmapHeight),Mathf.RoundToInt(x_01 * terrData.heightmapWidth) );

				// Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
				Vector3 normal = terrData.GetInterpolatedNormal(y_01,x_01);

				// Calculate the steepness of the terrain
				float steepness = terrData.GetSteepness(y_01,x_01);

				// Setup an array to record the mix of texture weights at this point
				float[] splatWeights = new float[terrData.alphamapLayers];

				// CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT

				// Texture[0] has constant influence
				splatWeights[0] = 0.5f;

				// Texture[2] stronger on flatter terrain
				// Note "steepness" is unbounded, so we "normalise" it by dividing by the extent of heightmap height and scale factor
				// Subtract result from 1.0 to give greater weighting to flat surfaces
				splatWeights[1] = 1.0f - Mathf.Clamp01(steepness*steepness/(terrData.heightmapHeight/5.0f));

				// Texture[3] increases with height but only on surfaces facing positive Z axis 
				splatWeights[2] = height * Mathf.Clamp01(normal.z);

				// Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
				float z = Sum(splatWeights);

				// Loop through each terrain texture
				for(int k = 0; k<terrData.alphamapLayers; k++){

					// Normalize so that sum of all texture weights = 1
					splatWeights[k] /= z;

					// Assign this point to the splatmap array
					splatmapData[i, j, k] = splatWeights[k];
				}
			}
		}
		// Finally assign the new splatmap to the terrainData:
		terrData.SetAlphamaps(x, y, splatmapData);
	}

	float Sum(float[] farr) {
		float f = 0.0f;
		for (int i = 0; i < farr.Length; ++i) {
			f += farr [i];
		}
		return f;
	}




}
