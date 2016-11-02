using UnityEngine;
using System.Collections;
using System.Diagnostics;


public class threeSidesCover : MonoBehaviour {


	//Vector3 [] covering = {new Vector3(0,0,0),new Vector3(0,0,1),new Vector3(-1,0,1),new Vector3(1,0,1),new Vector3(-1,0,2),new Vector3(0,0,2),new Vector3(1,0,2),new Vector3(-1,0,3),new Vector3(0,0,3),new Vector3(1,0,3),new Vector3(-1,0,4),new Vector3(0,0,4),new Vector3(1,0,4),new Vector3(0,0,5),new Vector3(1,0,-2),new Vector3(2,0,-2),new Vector3(3,0,-2),new Vector3(1,0,-3),new Vector3(2,0,-3),new Vector3(3,0,-3),new Vector3(4,0,-3),new Vector3(1,0,-4),new Vector3(2,0,-4),new Vector3(3,0,-4),new Vector3(4,0,-4),new Vector3(2,0,-5),new Vector3(3,0,-5),new Vector3(4,0,-5),new Vector3(-1,0,-2),new Vector3(-2,0,-2),new Vector3(-3,0,-2),new Vector3(-1,0,-3),new Vector3(-2,0,-3),new Vector3(-3,0,-3),new Vector3(-4,0,-3),new Vector3(-1,0,-4),new Vector3(-2,0,-4),new Vector3(-3,0,-4),new Vector3(-4,0,-4),new Vector3(-2,0,-5),new Vector3(-3,0,-5),new Vector3(-4,0,-5)};
	private RaycastHit currhit;
	private bool casted;
	private int layMask = 1<<8;
	private int LM = 1 << 10;
	private int[] distances = {2,4,6};
	private bool wasCovered;
	private int strength;
	private bool OnMouse = false;
	private NewCommutation commutation;


	//
	private RaycastHit[] hits1 = new RaycastHit[0];
	private RaycastHit[] hits2 = new RaycastHit[0];
	private RaycastHit[] hits3 = new RaycastHit[0];
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
			//hits1 = Physics.OverlapCapsule (transform.position, 5f, -1);
			hits1 = Physics.CapsuleCastAll (transform.position, transform.position+ new Vector3 (-6f,0,-6f),1f,transform.forward);
			for (int i = 0; i < hits1.Length; i++) {
				currhit = hits1 [i];
				if (currhit.transform.GetComponent<NodeMaterial> () != null && !Physics.Linecast (transform.position, currhit.transform.position, layMask)) {
					strength = calcStrength (Vector3.Distance (transform.position, currhit.transform.position));


					//print (CalcAngle.calcAngleBetween(transform.position, transform.position + Vector3.forward, transform.position, currhit.transform.position));
					//print (Vector3.Angle(currhit.transform.position,transform.position)*Mathf.Rad2Deg);

					//print (CalcAngle.AngleSighned (dirToNode, transform.forward,transform.forward));
					//print (CalcAngle.angle360(dirToNode,transform.forward, Vector3.right));


					currhit.transform.SendMessage ("Cover", strength);
				}
			}


			hits2 = Physics.CapsuleCastAll (transform.position, transform.position+ new Vector3 (0,0,7f),1.1f,transform.forward);
			for (int i = 0; i < hits2.Length; i++) {
				currhit = hits2 [i];
				if (currhit.transform.GetComponent<NodeMaterial> () != null && !Physics.Linecast (transform.position, currhit.transform.position, layMask)) {
					strength = calcStrength (Vector3.Distance (transform.position, currhit.transform.position));
					//тестовая часть


					Vector3 dirToNode = (currhit.transform.position - transform.position);
					if (currhit.transform.position.x < transform.position.x) {
						print (360f - Vector3.Angle (transform.forward, dirToNode));
					} else {
						print (Vector3.Angle (transform.forward, dirToNode));
					}
					//Vector3 dirToNode = (currhit.transform.position - transform.position);
					//print (360f - Vector3.Angle (transform.forward,dirToNode));
					//print (Vector3.Angle(transform.position,currhit.transform.position));

					//конец теста

					currhit.transform.SendMessage ("Cover", strength);
				}
			}

			hits3 = Physics.CapsuleCastAll (transform.position, transform.position + new Vector3 (6,0,-6f),1f,transform.forward);
			for (int i = 0; i < hits3.Length; i++) {
				currhit = hits3 [i];
				if (currhit.transform.GetComponent<NodeMaterial> () != null && !Physics.Linecast (transform.position, currhit.transform.position, layMask)) {
					strength = calcStrength (Vector3.Distance (transform.position, currhit.transform.position));
					currhit.transform.SendMessage ("Cover", strength);
				}
			}
		}
		wasCovered = true;
	}



	//вычисляем покрытие, которое надо убрать
	void calcUnCover()
	{ 
		if (wasCovered) {
			//hits1 = Physics.OverlapCapsule (transform.position, 5f, -1);
			hits1 = Physics.CapsuleCastAll (transform.position, transform.position+ new Vector3 (-6f,0,-6f),1f,transform.forward);
			for (int i = 0; i < hits1.Length; i++) {
				currhit = hits1 [i];
				if (currhit.transform.GetComponent<NodeMaterial> () != null && !Physics.Linecast (transform.position, currhit.transform.position, layMask)) {
					strength = calcStrength (Vector3.Distance (transform.position, currhit.transform.position));
					currhit.transform.SendMessage ("UnCover", strength);
				}
			}


			hits2 = Physics.CapsuleCastAll (transform.position, transform.position+ new Vector3 (0,0,7f),1.1f,transform.forward);
			for (int i = 0; i < hits2.Length; i++) {
				currhit = hits2 [i];
				if (currhit.transform.GetComponent<NodeMaterial> () != null && !Physics.Linecast (transform.position, currhit.transform.position, layMask)) {
					strength = calcStrength (Vector3.Distance (transform.position, currhit.transform.position));
					currhit.transform.SendMessage ("UnCover", strength);
				}
			}

			hits3 = Physics.CapsuleCastAll (transform.position, transform.position+ new Vector3 (6,0,-6f),1f,transform.forward);
			for (int i = 0; i < hits3.Length; i++) {
				currhit = hits3 [i];
				if (currhit.transform.GetComponent<NodeMaterial> () != null && !Physics.Linecast (transform.position, currhit.transform.position, layMask)) {
					strength = calcStrength (Vector3.Distance (transform.position, currhit.transform.position));
					currhit.transform.SendMessage ("UnCover", strength);
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