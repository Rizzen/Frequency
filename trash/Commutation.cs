using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Commutation : MonoBehaviour {

	public bool connected = false;
	private enum State
	{
		Disconnected, Connecting, Connected, Dragging
	}
	private State prevState;
	private State state;

	private Commutation outcomCommutation; //кеширование для компонента коммутации
	private Transform connectedTo; // к кому подключен
	private int layrMask = 1<<7;
	//public GameObject SpriteLine;
	private RaycastHit hit;
	public Transform target;
	//private Vector3 position;
	//private float angle;
	//private float distance;
	//private Object SpriteL1ne;

	private List<Transform> connectedList = new List<Transform>();
	private LineRenderer line;
	private Vector3[] positions;

	private bool OnMouse = false;
//	private Camera camera;


	#region States

	void stateDisconnected ()
	{
		//можно через свитч вписать логику, выполняющуюся по выходу из конкретного стейта
		// сет стейт
		//действие по входу в стейт
		//UnCommutate();
		Commutate(false);
	}

	void stateConnecting ()
	{
		//можно через свитч вписать логику, выполняющуюся по выходу из конкретного стейта
		// сет стейт
		//действие по входу в стейт
	}

	void stateConnected ()
	{
		//можно через свитч вписать логику, выполняющуюся по выходу из конкретного стейта
		// сет стейт
		//действие по входу в стейт
	}

	void stateDragging ()
	{
		//можно через свитч вписать логику, выполняющуюся по выходу из конкретного стейта
		// сет стейт
		//действие по входу в стейт
	}

	void setState (State value)
	{
		switch (state) 
		{
		case State.Connected:
			//действие по ыходу из стейта
			break;
		case State.Connecting:
			//действие по ыходу из стейта
			break;
		case State.Disconnected:
			//действие по ыходу из стейта
			break;
		case State.Dragging:
			//действие по ыходу из стейта
			break;
		}
		prevState = state;
		state = value;
	}
	#endregion


	#region Commutation


	#region Lines
	void DrawConnection (Transform target)
	{
		line.enabled = true;
		positions [1] = target.position;
		line.SetPositions (positions);
	}

	public void UpdateLine(Transform target)
	{
		//для обновления линии
		//line.enabled = true;
		positions [0] = transform.position;
		positions [1] = target.position;
		line.SetPositions (positions);

	}

	void UpdatePosition()
	{
		if (connectedList.Count > 0) {
			foreach (Transform commutated in connectedList) {
				commutated.SendMessage ("UpdateLine",transform.position);
			}
		}
	}

	void deleteLine()
	{
		line.enabled = false;
	}
	#endregion

	void Commutate (bool comm)
	{
		connected = comm;
		if (connected == true) {
			transform.SendMessage ("calcCover");
			DrawConnection (connectedTo);

		} 
		else {
			transform.SendMessage ("calcUnCover");
			deleteLine (); }
		UpdateConnectionList ();
		//и меняем стейт на Connected
	}

	void Uncommutate ()
	{
		
	}




	public bool IncomingConnection (Transform incoming)
	{
		//эта функция вызывается другой станцией, когда она подключается к этой, можно сделать булевой
		connectedList.Add (incoming);
		return true;
	}

	public void Disconnect ()
	{
		
	}

	void OutcomConection (Transform outcom)
	{
		outcomCommutation = outcom.GetComponent<Commutation> ();
		if (outcomCommutation != null && outcomCommutation.IncomingConnection (transform)) {
			connectedTo = outcom;
			connected = outcomCommutation.connected;
			Commutate (connected);
			//outcomCommutation.UpdateConnectionList ();

		} else {
			print ("fail");
		}
	}


	public void UpdateConnectionList () //ВКЛЮЧАЕМ/ОТКЛЮЧАЕМ СОЕДИННЕНЫЕ СТАНЦИИ
	{
		//перебираем connectedList и выполняем у них UpdateConnection
		if (connectedList.Count > 0) {
			foreach (Transform commutated in connectedList) {
				commutated.SendMessage ("UpdateConnection",connected);
			}
		}
	}



	void UpdateConnection (bool isConnected)
	{
		// вызывается внешней станцией при UpdateConnectionList

		if (connected != isConnected) {
			Commutate (isConnected);
		}
	
		if (connectedList.Count > 0)
		{
			UpdateConnectionList ();
		}
	}



	#endregion




	void Start () {

		connectedList = new List<Transform> ();
		line = transform.GetComponentInChildren<LineRenderer> ();
		positions = new Vector3[2];
		positions [0] = transform.position;
		state = State.Disconnected;
		prevState = State.Disconnected;
		Debug.Log (connectedList.Count);


		//DrawConnection (target);

	}


	void Update () {
		
		if (OnMouse) {
			if (Input.GetButtonDown ("Fire1")) {
				print ("Fire1");
				switch (state) 
				{
				case State.Connected:
					//setState (State.Dragging);
					//print ("State Connected Fire1");

					//действие по ыходу из стейта
					break;
				case State.Connecting:
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

					if (Physics.Raycast (ray, out hit)) {
						outcomCommutation = hit.transform.GetComponent<Commutation> ();
						if (outcomCommutation != null) {
							Transform objectHit = hit.transform;
							print ("Station");
							outcomCommutation.IncomingConnection (transform);
							OutcomConection (hit.transform);
						
						}
					}
					setState (State.Connected);
					//действие по ыходу из стейта
					break;
				case State.Disconnected:
					//действие по ыходу из стейта
					break;
				case State.Dragging:
					//действие по ыходу из стейта
					break;
				}
			}

			if (Input.GetButtonDown ("Fire2")) {
				//print ("Fire2");
				switch (state) 
				{
				case State.Connected:
					print ("State Connected fire2");
					setState (State.Connecting);
					break;
				case State.Connecting:
					//setState (prevState);
					break;
				case State.Disconnected:
					print ("State DisConnected fire 2");
					setState (State.Connecting);
					break;
				case State.Dragging:
					//действие по ыходу из стейта
					break;
				}
			}
			
		}
	}


	void OnMouseEnter()
	{

		OnMouse = true;

	}

	void OnMouseExit()
	{
		OnMouse = false;
	}



}


#region Fignya
////position = new Vector3 ((transform.position.x + target.position.x) / 2, (transform.position.y + target.position.y) / 2, (transform.position.z + target.position.z) / 2);
//distance = Vector3.Distance (transform.position, target.position);
//angle = Vector3.Angle (target.position, transform.position);

//SpriteLine = Instantiate (SpriteLine, transform.position + new Vector3(0,0,0), Quaternion.Euler(90,-angle,0)) as GameObject;
//SpriteLine.transform.localScale = new Vector3 (distance,1,1);
#endregion