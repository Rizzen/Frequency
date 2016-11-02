using UnityEngine;
using System.Collections;

public class ScoreCount : MonoBehaviour {


	private int Scores;
	// Use this for initialization
	void Awake () {
		Scores = 0;
//		print (Scores);
	}
	
	// Update is called once per frame
	void Update () {
		//print (Scores);
	
	}

	void ScoreUp(int x)
	{
		Scores += x;	
	}

	void ScoreDown(int x)
	{
		Scores -= x;
	}

}
