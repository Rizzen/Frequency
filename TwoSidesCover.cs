using UnityEngine;
using System.Collections;

public class TwoSidesCover : MonoBehaviour {

	Vector3 [] covering = {new Vector3(0,0,0),new Vector3(1,0,-1),new Vector3(1,0,0),new Vector3(1,0,1),new Vector3(2,0,-2),new Vector3(2,0,-1),new Vector3(2,0,0),new Vector3(2,0,1),new Vector3(2,0,2),new Vector3(3,0,-2),new Vector3(3,0,-1),new Vector3(3,0,0),new Vector3(3,0,1),new Vector3(3,0,2),new Vector3(4,0,-2),new Vector3(4,0,-1),new Vector3(4,0,0),new Vector3(4,0,1),new Vector3(4,0,2),new Vector3(5,0,-1),new Vector3(5,0,0),new Vector3(5,0,1),new Vector3(6,0,0),              new Vector3(-2,0,0),new Vector3(-3,0,-1),new Vector3(-3,0,0),new Vector3(-3,0,1),new Vector3(-4,0,-2),new Vector3(-4,0,-1),new Vector3(-4,0,0),new Vector3(-4,0,1),new Vector3(-4,0,2),new Vector3(-5,0,-1),new Vector3(-5,0,0),new Vector3(-5,0,1),new Vector3(-5,0,2),new Vector3(-5,0,-2),new Vector3(-6,0,-2),new Vector3(-6,0,-1),new Vector3(-6,0,0),new Vector3(-6,0,1),new Vector3(-6,0,2),new Vector3(-7,0,-1),new Vector3(-7,0,0),new Vector3(-7,0,1),new Vector3(-8,0,0) };//                                       
	RaycastHit currhit = new RaycastHit();
	private bool casted;
	private NewCommutation commutation;

	private Collider currHit;
	private int layMask = 1<<8;
	private int LM = 1 << 10;
	public int[] distances = {2,4,6};
	private bool wasCovered;
	private int strength;
	private bool OnMouse = false;


	private float angleBetween;


	//
	private Collider[] hits = new Collider[0];


	int calcStrength(float distance) //расчет силы покрытия
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



	void calcCover()
	{
		if (!wasCovered) {
			hits = Physics.OverlapSphere (transform.position, 10f, -1);

			for (int i = 0; i < hits.Length; i++) {
				currHit = hits [i];
				if (currHit.transform.GetComponent<NodeMaterial> () != null && !Physics.Linecast (transform.position, currHit.transform.position, layMask)) {

					Vector3 dirToNode = (currHit.transform.position - transform.position);
					if (currHit.transform.position.x < transform.position.x) {
						angleBetween = 360f - Vector3.Angle (transform.forward, dirToNode);
						//print (angleBetween);
					} else {
						angleBetween = Vector3.Angle (transform.forward, dirToNode);
						//print (angleBetween);
					}

					if ((angleBetween >= 71f && angleBetween <= 108f) || (angleBetween >= 247f && angleBetween <= 292f))  {
						strength = calcStrength (Vector3.Distance (transform.position, currHit.transform.position));
						currHit.SendMessage ("Cover", strength);
					}
				}
			}
		}
		wasCovered = true;

	}

	void calcUnCover()
	{
		if (wasCovered) {
			hits = Physics.OverlapSphere (transform.position, 10f, -1);

			for (int i = 0; i < hits.Length; i++) {
				Collider currHit = hits [i];
				if (currHit.transform.GetComponent<NodeMaterial> () != null && !Physics.Linecast (transform.position, currHit.transform.position, layMask)) {

					Vector3 dirToNode = (currHit.transform.position - transform.position);
					if (currHit.transform.position.x < transform.position.x) {
						angleBetween = 360f - Vector3.Angle (transform.forward, dirToNode);
						//print (angleBetween);
					} else {
						angleBetween = Vector3.Angle (transform.forward, dirToNode);
						//print (angleBetween);
					}

					if ((angleBetween >= 71f && angleBetween <= 108f) || (angleBetween >= 247f && angleBetween <= 292f)) {
						strength = calcStrength (Vector3.Distance (transform.position, currHit.transform.position));
						currHit.SendMessage ("UnCover", strength);
					}
				}
			}
		}
		wasCovered = false;
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

	// Use this for initialization
	void Start () {
		wasCovered = false;
		commutation = transform.GetComponent<NewCommutation> ();
		//commutation.connected = false;
		OnMouse = false;
		//calcCover ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
