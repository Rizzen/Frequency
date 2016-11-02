using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class RayCastCovering : MonoBehaviour {

	//для покрытия
	private Collider[] hits = new Collider[0];
	private Collider currHit = new Collider();
	private int[] distances = { 2, 4, 5 }; //расстояния, на которых меняется сила покрытия
	private int layMask = 1<<8; // маска для лайнкаста. в 8 слое располагаются коллизии

	private int strength; // кеширование для вычисления силы
	private NewCommutation commutation;
	private bool wasCovered = false;




	private LineRenderer line;
	private Vector3[] target;
	private bool OnMouse;

	#region Cover
	int calcStrength(float distance)
	{
		strength = Mathf.RoundToInt (distance);
		if (distance <= distances [0]) {
			return 3;
		} else if (distance > distances [0] && distance <= distances [1]) {
			return 2;
		} else {
			return 1;
		}
	}

	public void calcCover()
	{
		if (!wasCovered) {
			hits = Physics.OverlapSphere (transform.position, 5f, -1);
			for (int i = 0; i < hits.Length; i++) {
				currHit = hits [i];
				if (currHit.transform.GetComponent<NodeMaterial> () != null && !Physics.Linecast (transform.position, currHit.transform.position, layMask)) {
					strength = calcStrength (Vector3.Distance (transform.position, currHit.transform.position));
					currHit.SendMessage ("Cover", strength);
				}
			}
		}
		wasCovered = true;
	}

	public void calcUnCover()
	{
		if (wasCovered)
		{
		hits = Physics.OverlapSphere (transform.position, 5f, -1);

		for (int i = 0; i < hits.Length; i++) {
			Collider currHit = hits[i];
			if (currHit.transform.GetComponent<NodeMaterial> () != null && !Physics.Linecast(transform.position,currHit.transform.position,layMask)) {
				strength = calcStrength(Vector3.Distance (transform.position, currHit.transform.position));
				currHit.SendMessage ("UnCover", strength);
			}
		}
			wasCovered = false;
	}
	}
	#endregion

	void Start () {
		commutation = transform.GetComponent<NewCommutation> ();
		//commutation.connected = false;
		OnMouse = false;
	}

	void Update ()
	{
		if (OnMouse && Input.GetButton ("Fire1")) {
			transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
		} else if (OnMouse && Input.GetButton("Fire2")) {
			//print (gameObject.GetInstanceID());


		}
	}

	#region Input
	void OnMouseEnter()
	{
		OnMouse = true;
	}

	void OnMouseExit()
	{
		OnMouse = false;
	}

	void OnMouseDown()
	{
	    transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
		if (commutation.connected) {
			calcUnCover ();

		}
	}

	void OnMouseUp()
	{
		transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
		if (commutation.connected) {
			calcCover ();
		}
	}
	#endregion
}

//line = transform.GetComponentInChildren<LineRenderer> ();
//line.enabled = true;
//line.SetPositions (target);