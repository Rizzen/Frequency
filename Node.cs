using UnityEngine;
using System.Collections;

public class Node {

	public bool covered;//состояние ноды (-1 - маяк, 0 - не покрыта, 1 - покрыта)
	public Vector3 worldPos;
	public int gridX;
	public int gridY;
	public Material coveredMat;
	public Material unCoveredMat;




	public Node (bool _covered, Vector3 _worldPos, int _gridX, int _gridY)
	{ 
		covered = _covered;
		worldPos = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	void Update()
	{
		
	}

	

}
