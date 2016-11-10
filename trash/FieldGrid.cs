using UnityEngine;
using System.Collections;

public class FieldGrid : MonoBehaviour {

	public int gridSizeX;
	public int gridSizeY;
	public float nodeRadius;
	public GameObject Node;
	Node[,] grid;
	float nodeDiam;
	int gridX, gridY;
	int count = 0;
	 

	// Use this for initialization
	void Start () {
		
		nodeDiam = nodeRadius * 2;
		gridX = Mathf.RoundToInt (gridSizeX / nodeDiam);
		gridY = Mathf.RoundToInt (gridSizeY / nodeDiam);
		CreateGrid ();
	}


	void CreateGrid ()
	{
		grid = new Node[gridX,gridY];
		//Vector3 worldBottomLeft = Vector3.zero - (Vector3.right * gridSizeX / 2) - (Vector3.forward * gridSizeY / 2);//нижний левый угол, сам объект надодится в центре сетки

		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				Vector3 worldPos = Vector3.zero + Vector3.right * (x * nodeDiam + nodeRadius) + Vector3.forward * (y * nodeDiam + nodeRadius);
				grid [x, y] = new Node (false, worldPos, x, y);
				Instantiate (Node, worldPos+ new Vector3 (0.5f,0,0.5f)+(Vector3.up*0.5f),Quaternion.AngleAxis(90,Vector3.right));
				count++;


			}
		}
		//print (count);
	}

	public Node NodeFromPosition (Vector3 worldPosition)

	{
		
		float percentX = (worldPosition.x +gridSizeX/2)/gridSizeX;
		float percentY = (worldPosition.z +gridSizeY/2)/gridSizeY;

		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);


		int x = Mathf.RoundToInt ((gridX-1)*percentX);
		int y = Mathf.RoundToInt ((gridY-1)*percentY);
		Debug.Log ("X " + x + " Y " + y);

		return grid [x, y];
	}



	// Update is called once per frame
	void Update () 
	{
	  
	}
}
