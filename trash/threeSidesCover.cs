using UnityEngine;
using System.Collections;
using System.Diagnostics;


public class threeSidesCover : MonoBehaviour {



	private Collider currHit;
	private bool casted;
	private int layMask = 1<<8;
	private int LM = 1 << 10;
	public int[] distances = {2,4,6};
	private bool wasCovered;
	private int strength;
	private bool OnMouse = false;
	private NewCommutation commutation;

	private float angleBetween;


	//
	private Collider[] hits = new Collider[0];

	// покрытие через 

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

	//
	#region Covering
	//вычисляем покрытие
	void calcCover()
	{
		if (!wasCovered) {
			hits = Physics.OverlapSphere (transform.position, 12f, -1);
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

					if ((angleBetween >= 340f && angleBetween <= 360f) || (angleBetween >= 0f && angleBetween <= 20f) || (angleBetween >= 105f && angleBetween <= 145f) || (angleBetween >= 215f && angleBetween <= 255f)) {
						strength = calcStrength (Vector3.Distance (transform.position, currHit.transform.position));
						currHit.SendMessage ("Cover", strength);
					}
				}
			}
		}
		wasCovered = true;
	}

	//вычисляем покрытие, которое надо убрать
	void calcUnCover()
	{ 
		if (wasCovered) {
			hits = Physics.OverlapSphere (transform.position, 12f, -1);

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

					if ((angleBetween >= 340f && angleBetween <= 360f) || (angleBetween >= 0f && angleBetween <= 20f) || (angleBetween >= 105f && angleBetween <= 145f) || (angleBetween >= 215f && angleBetween <= 255f)) {
						strength = calcStrength (Vector3.Distance (transform.position, currHit.transform.position));
						currHit.SendMessage ("UnCover", strength);
					}
				}
			}
		}
		wasCovered = false;
	}
	#endregion



	void Start () {
		wasCovered = false;
		commutation = transform.GetComponent<NewCommutation> ();
		//commutation.connected = false;
		OnMouse = false;
		//currhit = new RaycastHit();
		//calcCover ();
		//print(CalcAngle.calcAngleBetween(new Vector3(1f,4f,5f),new Vector3(3f,1f,1f),new Vector3(2f,1f,1f) ,new Vector3(7f,2f,1f)  ));
	}

	void Update () {
		transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
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
		if (commutation.online) {
			calcUnCover ();

		}
	}

	void OnMouseUp()
	{
		transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
		if (commutation.online) {
			calcCover ();
		}
	}
	#endregion

}