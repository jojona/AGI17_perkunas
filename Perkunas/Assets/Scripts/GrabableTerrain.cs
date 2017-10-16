using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableTerrain : Grabable {
	GameObject manipulator = null;

	public float effectRadius = 4.0f;
	private float resolutionPerUnit;
	public Terrain terrain;
	private int cubeWidth;
	public float terrainWidth = 10.0f;
	public float terrainMaxHeight = 3.0f;

	private int textureFpscounter = 0;
	private float starttime = 0.0f;
	private float updatetime = 0.5f;


	private float[,] precalcWeigths;
	// Use this for initialization
	void Start () {
		if (terrain == null) {
			terrain = GetComponent<Terrain> ();
		}
		if (terrain == null) {
			Debug.Log ("no terrain in GrabableTerrain");
			return;
		}
		resolutionPerUnit = ((float)terrain.terrainData.heightmapResolution) / terrainWidth;//((float)terrain.terrainData.heightmapWidth);
		cubeWidth = (int)(effectRadius * resolutionPerUnit);
		precalcWeigths = new float[cubeWidth, cubeWidth];
		int maxDist = cubeWidth / 2;
		//we precaclulate the weights of each tile in the terrain mnodification radius, so we don't have to do so at runtime
		for (int x = 0; x < cubeWidth; ++x) {
			for (int y = 0; y < cubeWidth; ++y) {
				float dist = (x - (maxDist))*(x-(maxDist));
				dist += (y - (maxDist)) * (y - (maxDist));
				dist = Mathf.Sqrt (dist);
				precalcWeigths [x, y] = (float)Mathf.Cos ((float)((dist / (float)maxDist) * 0.5 * Mathf.PI));
			}
		}

		//create very simple "standard" terrain
		float[,] terr = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];

		maxDist = terrain.terrainData.heightmapWidth / 2;
		for (int x = 0; x < terrain.terrainData.heightmapWidth; ++x) {
			for (int y = 0; y < terrain.terrainData.heightmapHeight; ++y) {
				float dist = (x - (maxDist))*(x-(maxDist));
				dist += (y - (maxDist)) * (y - (maxDist));
				dist = Mathf.Sqrt (dist);
				terr [x, y] = (float)Mathf.Cos ((float)((dist / (float)maxDist) * 0.5 * Mathf.PI))*(1.0f/terrainMaxHeight);
			}
		}
		// Flatten the terrain
		terrain.terrainData.SetHeights (0, 0, terr);

		updateTextureInSquare (0.0f, 0.0f, 1.0f);
	}


	private float max(float a, float b) {
		return a > b ? a : b;
	}
	private float min(float a, float b) {
		return a < b ? a : b;
	}
	// Update is called once per frame
	void Update () {
		textureFpscounter = (textureFpscounter + 1) % 15;


		if (manipulator != null && terrain != null) {
			Vector2 manipRelPos = new Vector2 (manipulator.transform.position.x - transform.position.x, manipulator.transform.position.z- transform.position.z);
			float manipRelHeight = manipulator.transform.position.y - transform.position.y;
			int absX = (int)(manipRelPos.x * resolutionPerUnit);
			int absY = (int)(manipRelPos.y * resolutionPerUnit);
			float newHeight = manipRelHeight / terrainMaxHeight;
			newHeight = newHeight > 1 ? 1 : newHeight;
			newHeight = newHeight < 0 ? 0 : newHeight;
			//Debug.Log ("relative position: "+ manipRelPos + ", relative height: " + manipRelHeight + ", relative height abs: " + relHeightAbs);
			//Debug.Log ("absX: "+ absX + ", absY: " + absY);
			//Debug.Log ("resu: "+ resolutionPerUnit);
			int maxDist = cubeWidth/2;
			float offset = (float)0.025 / terrainMaxHeight;
			if(newHeight - offset > terrain.terrainData.GetHeight(absX, absY)/terrainMaxHeight) {
				newHeight -= offset;
				float[,] data = terrain.terrainData.GetHeights (absX - maxDist, absY - maxDist, cubeWidth, cubeWidth);
				//Debug.Log("terrain height1: " + data[maxDist,maxDist]);
				for (int x = 0; x < cubeWidth; ++x) {
					for (int y = 0; y < cubeWidth; ++y) {
						data [x, y] = max(newHeight * precalcWeigths [x, y], data [x, y]);
						//data [x, y] = data [x, y] < 0.0f ? 0.0f : data [x, y];//I don't think we deen to clamp the values
						//data [x, y] = data [x, y] > 1.0f ? 1.0f : data [x, y];
					}
				}
				//Debug.Log ("set to: " + data [maxDist, maxDist]);

				terrain.terrainData.SetHeights(absX - maxDist, absY - maxDist, data);

				if (Time.time - starttime > updatetime ) {// textureFpscounter == 0) {
					starttime = Time.time;
					updateTextureInSquare (((float)(absX - maxDist)) / (float)terrain.terrainData.heightmapWidth,
						((float)(absY - maxDist)) / (float)terrain.terrainData.heightmapWidth,
						((float)cubeWidth) / (float)terrain.terrainData.heightmapWidth);
				}
			} else if(newHeight + offset < terrain.terrainData.GetHeight(absX, absY)/terrainMaxHeight){
				newHeight += offset;
				float[,] data = terrain.terrainData.GetHeights (absX - maxDist, absY - maxDist, cubeWidth, cubeWidth);
				//Debug.Log("terrain height2: " + data[0,0]);
				for (int x = 0; x < cubeWidth; ++x) {
					for (int y = 0; y < cubeWidth; ++y) {
						data [x, y] = min(1.0f - max((1.0f-newHeight) * precalcWeigths [x, y],0.0f), data [x, y]);
						//data [x, y] = data [x, y] < 0.0f ? 0.0f : data [x, y];
						//data [x, y] = data [x, y] > 1.0f ? 1.0f : data [x, y];
					}
				}
				//Debug.Log ("set to: " + data [0, 0]);

				terrain.terrainData.SetHeights(absX - maxDist, absY - maxDist, data);
				if (Time.time - starttime > updatetime ) {// textureFpscounter == 0) {
					starttime = Time.time;
					updateTextureInSquare (((float)(absX - maxDist)) / (float)terrain.terrainData.heightmapWidth,
						((float)(absY - maxDist)) / (float)terrain.terrainData.heightmapWidth,
						((float)cubeWidth) / (float)terrain.terrainData.heightmapWidth);
				}
			}
		}
	}

	public override void attach(GameObject g) {
		manipulator = g;
		starttime = Time.time;
	}

	public override void detach(GameObject g, Vector3 a, Vector3 b) {
		// TODO remove code duplication
		Vector2 manipRelPos = new Vector2 (manipulator.transform.position.x - transform.position.x, manipulator.transform.position.z- transform.position.z);
		int absX = (int)(manipRelPos.x * resolutionPerUnit);
		int absY = (int)(manipRelPos.y * resolutionPerUnit);
		int maxDist = cubeWidth/2;
		updateTextureInSquare (((float)(absX - maxDist)) / (float)terrain.terrainData.heightmapWidth,
			((float)(absY - maxDist)) / (float)terrain.terrainData.heightmapWidth,
			((float)cubeWidth) / (float)terrain.terrainData.heightmapWidth);



		manipulator = null;
		//we do nothing special with the release speeds here
	}

	public void updateTextureInSquare(float x_01, float y_01, float width_01) {
		TerrainData terrData = terrain.terrainData;
		int x = (int)(x_01 * terrData.alphamapWidth);
		int y = (int)(y_01 * terrData.alphamapHeight);
		int xMax = (int)((x_01 + width_01) * terrData.alphamapWidth);
		int yMax = (int)((y_01 + width_01) * terrData.alphamapHeight);

		//clamp to sane values
		xMax = xMax > terrData.alphamapWidth ? terrData.alphamapWidth : xMax;
		yMax = yMax > terrData.alphamapHeight ? terrData.alphamapHeight : yMax;
		x = x < 0 ? 0 : x;
		y = y < 0 ? 0 : y;


		float[, ,] splatmapData = new float[ yMax - y, xMax - x,terrData.alphamapLayers];
		float[] splatWeights = new float[terrData.alphamapLayers];//this is declared outside loop to reduce memory allocation in loop
		float deltaX = 1.0f / (float)terrData.alphamapWidth;
		float deltaY = 1.0f / (float)terrData.alphamapHeight;


		float maxh = 0.0f;
		//x_01 = (float)x/(float)terrData.alphamapWidth;
		float ystore = y_01;
		for (int i = x; i < xMax; ++i, x_01 += deltaX) {
			y_01 = ystore;
			for (int j = y; j < yMax; ++j, y_01 += deltaY) {
				//code from here copy-pasted from tutorial, make sure to update it as apropriate
				//variable reuse lol
				//y_01 = (float)j/(float)terrData.alphamapHeight;
				//x_01 = (float)i/(float)terrData.alphamapWidth;

				// Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
				float height = terrData.GetHeight(i,j ) / terrData.size.y;
				if (height > maxh)
					maxh = height;
				// Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
				//Vector3 normal = terrData.GetInterpolatedNormal(y_01,x_01);

				// Calculate the steepness of the terrain
				float steepness = terrData.GetSteepness(x_01,y_01);


				steepness = steepness / 90.0f;

				//turn steepness into something useful
				steepness = 1.0f - steepness;
				steepness *= steepness;
				steepness = 1.0f - steepness;


				splatWeights[0] = (1.0f- steepness)*(1.0f - height);


				splatWeights[1] = steepness;


				splatWeights[2] = (1.0f- steepness)*height;

				splatWeights [3] = 0.0f;

				// Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
				float z = Sum(splatWeights);

				// Loop through each terrain texture
				for(int k = 0; k<terrData.alphamapLayers; k++){

					// Normalize so that sum of all texture weights = 1
					splatWeights[k] /= z;

					// Assign this point to the splatmap array
					//splatmapData[i-x, j-y, k] = splatWeights[k];
					splatmapData[ j-y, i-x,k] = splatWeights[k];
				}
			}
		}
		// Debug.Log ("max height was: " + maxh);
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
