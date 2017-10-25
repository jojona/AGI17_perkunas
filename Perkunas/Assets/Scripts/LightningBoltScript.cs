
using UnityEngine;
using System.Collections.Generic;

public enum LightningBoltAnimationMode
{
    // No animation
    None,

    // Pick a random frame
    Random,

    // Loop through each frame and restart at the beginning
    Loop,

    // Loop through each frame then go backwards to the beginning then forward, etc.
    PingPong
}

// Allows creation of simple lightning bolts
[RequireComponent(typeof(LineRenderer))]
public class LightningBoltScript : MonoBehaviour
{
	//the starting position of the ligthning
    public Vector3 StartPosition;
	//the ending position of the lightning
    public Vector3 EndPosition;

	public LayerMask treeMask;

    // numbers of line segments
    public int Generations = 6;
	// duration of the lightning
    public float Duration = 0.05f;
    private float timer;
	//Chaos inside the lightning
	public float ChaosFactor = 0.15f;
	// trigger mode
    public bool ManualMode;

    public int Rows = 1;
    public int Columns = 1;


    public LightningBoltAnimationMode AnimationMode = LightningBoltAnimationMode.PingPong;

    public System.Random RandomGenerator = new System.Random();

    private LineRenderer lineRenderer;

	private List<KeyValuePair<Vector3, Vector3>> segments = new List<KeyValuePair<Vector3, Vector3>>();
    private int startIndex;
    private Vector2 size;
    private Vector2[] offsets;
    private int animationOffsetIndex;
    private int animationPingPongDirection = 1;

	private Vector3 GetPerpendicularVector(Vector3 directionNormalized)
    {
        if (directionNormalized == Vector3.zero)
        {
            return Vector3.right;
        }
        else
        {
            float x = directionNormalized.x;
            float y = directionNormalized.y;
            float z = directionNormalized.z;
            float px, py, pz;
            float ax = Mathf.Abs(x), ay = Mathf.Abs(y), az = Mathf.Abs(z);
            if (ax >= ay && ay >= az)
            {
                // x is the max, so we can pick (py, pz) arbitrarily at (1, 1):
                py = 1.0f;
                pz = 1.0f;
                px = -(y * py + z * pz) / x;
            }
            else if (ay >= az)
            {
                // y is the max, so we can pick (px, pz) arbitrarily at (1, 1):
                px = 1.0f;
                pz = 1.0f;
                py = -(x * px + z * pz) / y;
            }
            else
            {
                // z is the max, so we can pick (px, py) arbitrarily at (1, 1):
                px = 1.0f;
                py = 1.0f;
                pz = -(x * px + y * py) / z;
            }
			Vector3 side = new Vector3(px, py, pz).normalized;
			return side;
        }
    }

    private void GenerateLightningBolt(Vector3 start, Vector3 end, int generation, int totalGenerations, float offsetAmount)
    {
	//Debug.Log ("Pour la vraie, la position de départ vaut : "+StartPosition+ ", la position final vaut : "+EndPosition);

        if (generation < 0 || generation > 8)
        {
            return;
        }

        segments.Add(new KeyValuePair<Vector3, Vector3>(start, end));
        if (generation == 0)
        {
            return;
        }

        Vector3 randomVector;
        if (offsetAmount <= 0.0f)
        {
            offsetAmount = (end - start).magnitude * ChaosFactor;
        }

        while (generation-- > 0)
        {
            int previousStartIndex = startIndex;
            startIndex = segments.Count;
            for (int i = previousStartIndex; i < startIndex; i++)
            {
                start = segments[i].Key;
                end = segments[i].Value;

                // determine a new direction for the split
                Vector3 midPoint = (start + end) * 0.5f;

                // adjust the mid point to be the new location
                randomVector = RandomVector( start, end, offsetAmount );
                midPoint += randomVector;

                // add two new segments
                segments.Add(new KeyValuePair<Vector3, Vector3>(start, midPoint));
                segments.Add(new KeyValuePair<Vector3, Vector3>(midPoint, end));
            }

            // halve the distance the lightning can deviate for each generation down
            offsetAmount *= 0.5f;
        }
    }

	public Vector3 RandomVector(Vector3 start, Vector3 end, float offsetAmount )
    {
		Vector3 result = new Vector3 ();
            Vector3 directionNormalized = (end - start).normalized;
            Vector3 side = GetPerpendicularVector(directionNormalized);

            // generate random distance
            float distance = (((float)RandomGenerator.NextDouble() + 0.1f) * offsetAmount);

            // get random rotation angle to rotate around the current direction
            float rotationAngle = ((float)RandomGenerator.NextDouble() * 360.0f);

            // rotate around the direction and then offset by the perpendicular vector
            result = Quaternion.AngleAxis(rotationAngle, directionNormalized) * side * distance;
		return result;
    }

    private void SelectOffsetFromAnimationMode()
    {
        int index;

        if (AnimationMode == LightningBoltAnimationMode.None)
        {
            lineRenderer.material.mainTextureOffset = offsets[0];
            return;
        }
        else if (AnimationMode == LightningBoltAnimationMode.PingPong)
        {
            index = animationOffsetIndex;
            animationOffsetIndex += animationPingPongDirection;
            if (animationOffsetIndex >= offsets.Length)
            {
                animationOffsetIndex = offsets.Length - 2;
                animationPingPongDirection = -1;
            }
            else if (animationOffsetIndex < 0)
            {
                animationOffsetIndex = 1;
                animationPingPongDirection = 1;
            }
        }
        else if (AnimationMode == LightningBoltAnimationMode.Loop)
        {
            index = animationOffsetIndex++;
            if (animationOffsetIndex >= offsets.Length)
            {
                animationOffsetIndex = 0;
            }
        }
        else
        {
            index = RandomGenerator.Next(0, offsets.Length);
        }

        if (index >= 0 && index < offsets.Length)
        {
            lineRenderer.material.mainTextureOffset = offsets[index];
        }
        else
        {
            lineRenderer.material.mainTextureOffset = offsets[0];
        }
    }

	private void UpdateLineRenderer(LineRenderer line)
    {
        int segmentCount = (segments.Count - startIndex) + 1;
        line.positionCount = segmentCount;

        if (segmentCount < 1)
        {
            return;
        }

        int index = 0;
        line.SetPosition(index++, segments[startIndex].Key);

        for (int i = startIndex; i < segments.Count; i++)
        {
            line.SetPosition(index++, segments[i].Value);
        }

        segments.Clear();

        SelectOffsetFromAnimationMode();
    }



    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        UpdateFromMaterialChange();
    }

    private void Update()
    {
        if (timer <= 0.0f)
        {
            if (ManualMode)
            {
                timer = Duration;
                lineRenderer.positionCount = 0;
            }
            else
            {
                Trigger();
            }
        }
        timer -= Time.deltaTime;
    }

    // Trigger a lightning bolt. Use this if ManualMode is true.
    public void Trigger()
    {
        Vector3 start, end;
        timer = Duration + Mathf.Min(0.0f, timer);
            start = StartPosition;
            end = EndPosition;

        startIndex = 0;
        GenerateLightningBolt(start, end, Generations, Generations, 0.0f);
		addFire ();
		UpdateLineRenderer(lineRenderer);

	}

    // Call this method if you change the material on the line renderer
    public void UpdateFromMaterialChange()
    {
        size = new Vector2(1.0f / (float)Columns, 1.0f / (float)Rows);
        lineRenderer.material.mainTextureScale = size;
        offsets = new Vector2[Rows * Columns];
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                offsets[x + (y * Columns)] = new Vector2((float)x / Columns, (float)y / Rows);
            }
        }
    }
	void OnTriggerEnter(Collider other) 
	{
		Debug.Log ("Coucou");
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			other.gameObject.SetActive (false);
		}
	}

	public void addFire(){
		Debug.Log ("Coucou");
		RaycastHit hit;
		Vector3 direction = EndPosition - StartPosition;
		//Debug.Log ("Le laser va partir vers de : " + StartPosition+ " vers : " + direction + " sur une distance de " + direction.magnitude);
		if(Physics.Raycast(StartPosition + new Vector3 (0,5,0), direction, out hit, direction.magnitude +10, treeMask)){
			hit.collider.gameObject.GetComponent<setOnFire> ().getChild().SetActive(true);
			Debug.Log("On a touche avec "  + gameObject.name  + " l'objet : " +hit.collider.gameObject.name + " a la position : " + hit.point);

		}
	}
}
