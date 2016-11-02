using UnityEngine;
using System.Collections;

public class DragNDrop : MonoBehaviour {
	
	private bool dragging = false;
	private float distance;


	void Start () {
		transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
	}

	void OnMouseDown ()
	{
		distance = Vector3.Distance (transform.position, Camera.main.transform.position);

	}

	void OnMouseDrag()
	{
		dragging = true;
	}

	void OnMouseUp ()
	{
		dragging = false;
	}
	// Update is called once per frame
	void Update () {
		
		if (dragging) 
		{
			
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); // проекция с позиции мышки на экране на плоскость в мире
			Vector3 rayPoint = ray.GetPoint (distance); // определяем расстояние 
			transform.position = new Vector3 (rayPoint.x,transform.position.y,rayPoint.z);
		}
		if (!dragging) 
		{
			transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
		}
	}
}
