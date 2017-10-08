using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeModification : MonoBehaviour {

	private Mesh meshSauv;
	private int step;
	private int count = 0;
	// Use this for initialization
	void Start () {
		meshSauv = GetComponent<MeshFilter>().mesh;
		step = 0;
		Vector3[] vertices = meshSauv.vertices;
//		for (int i = 0; i < vertices.Length; i++) {
//			Debug.Log ("Voici les coordonnes du vecteur "+ vertices [i] [0] + " " + vertices [i] [1] + " " + vertices [i] [2]);
//		}
	//	for(int i = 0; i < vertices.Length; i++){
	//		vertices[i][0] = Mathf.SqrtMathf.Abs(vertices[i][1]+transform.position.y))/(Mathf.Exp(Mathf.Abs(vertices[i][1]+ transform.position.y)));
	//	}
		GetComponent<MeshFilter>().mesh.vertices = vertices;
		GetComponent<MeshFilter>().mesh.RecalculateBounds();
		count++;
	}

	void Update() {
		
		Vector3[] vertices = meshSauv.vertices;
		if (step % 100 == 0) {
			for (int i = 0; i < vertices.Length; i++) {
				if (vertices [i] [1] > 0) {
					float rayon = 1 - Mathf.Sqrt (vertices [i] [1] * 2);

				//	Debug.Log ("le point es t à la coordonnee x : " + vertices [i] [0] + " et z: " + vertices [i] [2]);
				//	Debug.Log ("les nouvelles coordonnees sont x : " + vertices [i] [0] / rayon + " et z : " + vertices [i] [2] / rayon);

				//	vertices [i] [0] /= rayon;
				//	vertices [i] [2] /= rayon; 
				}

			}
		}
		step++;



		GetComponent<MeshFilter>().mesh.vertices = vertices;
		GetComponent<MeshFilter>().mesh.RecalculateBounds();
	}

	Vector3 Noise(Vector3 vec){
		float i, j, k;
		Vector3 res =  new Vector3(0,0,0);
		i = vec [0];
		j = vec [1];
		k = vec [2];
		res [0] = Mathf.Sin (i * j + Time.time)/10;
		res [1] = Mathf.Cos (j * k + Time.time)/10;
		res [2] = Mathf.Sin (i * k + Time.time)/10;
		return res;
	}
}
