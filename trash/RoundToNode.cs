using UnityEngine;
using System.Collections;

public class RoundToNode : MonoBehaviour {


	public Transform cubeSelf;
	private bool dragging;

	// Use this for initialization
	void Start () {
		dragging = false;

	}

	void OnMouseDown()
	{
		dragging = true;

	}

	void OnMouseUp()
	{
		dragging = false;

	}

	// Update is called once per frame
	void Update () {
		//Вынести нах весь скрипт в DragNDrop
		if (!dragging) 
		{
			cubeSelf.transform.position = new Vector3 (Mathf.Round (cubeSelf.transform.position.x), cubeSelf.transform.position.y, Mathf.Round (cubeSelf.transform.position.z));
		}
	}
}
