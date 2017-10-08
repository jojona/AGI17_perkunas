using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

	public GameObject terrainObject;
	private GrabableTerrain grabTerrain;
	private Terrain terrain;

	private float heightmapPosX;
	private float heightmapPosZ;
	// Use this for initialization
	void Start () {
		terrain = terrainObject.GetComponent<Terrain> ();
		grabTerrain = terrain.GetComponent ("GrabableTerrain") as GrabableTerrain;

		heightmapPosX = transform.position.x / grabTerrain.terrainWidth * terrain.terrainData.heightmapWidth;
		heightmapPosZ = transform.position.z / grabTerrain.terrainWidth * terrain.terrainData.heightmapWidth;

		float y = terrain.terrainData.GetHeight ((int)heightmapPosX, (int)heightmapPosZ);
		transform.Translate (new Vector3(transform.position.x, y, transform.position.z)- transform.position);

	}
	
	// Update is called once per frame
	void Update () {
		// Update y - position based on terrain
		float y = terrain.terrainData.GetHeight ((int)heightmapPosX, (int)heightmapPosZ);
		transform.Translate ( new Vector3(transform.position.x, y, transform.position.z) -transform.position);

	}
}
