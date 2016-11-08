﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewCommutation : MonoBehaviour {
	
	public bool connected = false;
	//стейты
	private enum State
	{
		Disconnected, Connecting, Connected, Dragging
	}
	private State prevState;
	private State state;

	private Transform connectedTo = null;

	private List<Transform> connectedList = new List<Transform>();
	private LineRenderer line;
	private Vector3[] positions;
	private bool OnMouse = false;
	bool connecting = false;
	private RayCastCovering coveringScript;
	bool dragging = false;


	#region States

	void stateDisconnected ()
	{
		//можно через свитч вписать логику, выполняющуюся по выходу из конкретного стейта
		DisConnect ();
		//действие по входу в стейт
		setState(State.Disconnected);
		//print ("StateDisconnected ON");

	}

	void stateConnecting ()
	{
		//можно через свитч вписать логику, выполняющуюся по выходу из конкретного стейта
		//действие по входу в стейт
		if (connectedTo != null) {
			DisConnect ();
		}
		setState(State.Connecting);
		//print ("StateConnecting ON");
	}

	void stateConnected (Transform target)
	{
		//можно через свитч вписать логику, выполняющуюся по выходу из конкретного стейта
		//действие по входу в стейт
		setState(State.Connected);
		Connect (target);
		DrawConnection (target);
		//print ("StateConnected ON");
	}

	void stateDragging ()
	{
		setState (State.Dragging);
		//действие по входу в стейт
		//print ("StateDragging On");
	}

	void setState (State value)
	{
		switch (state) 
		{
		case State.Connected:
			//print ("StateConnected OFF");
			//действие по ыходу из стейта
			break;
		case State.Connecting:
			//print ("StateConnecting OFF");
			//действие по ыходу из стейта
			break;
		case State.Disconnected:
			//print ("StateDisconnected OFF");
			//действие по ыходу из стейта
			break;
		case State.Dragging:
			//print ("StateDragging Off");
			//действие по ыходу из стейта
			break;
		}
		prevState = state;
		state = value;
	}
	#endregion
	#region Lines
	void DrawConnection (Transform target)
	{
		line.enabled = true;
		positions [0] = transform.position + new Vector3(0,0.4f,0);
		positions [1] = target.position + new Vector3(0,0.4f,0);
		line.SetPositions (positions);
	}

	public void UpdateLine(Transform target)
	{
		//для обновления линии
		line.enabled = true;
		positions [0] = transform.position + new Vector3(0,0.4f,0);
		positions [1] = target.position + new Vector3(0,0.4f,0);
		line.SetPositions (positions);

	}

	void UpdatePosition()
	{

	}

	void deleteLine()
	{
		line.enabled = false;
	}
	#endregion
	#region Commutation



	public void AddConnection(Transform incoming)
	{
		connectedList.Add (incoming);
		//эта функция вызывается извне, подключающейся станцией
	}

	public void RemoveConnection(Transform incoming)
	{
		if (connectedList.Count > 0) {
			for (int i = 0; i < connectedList.Count; i++) {
				if (connectedList[i] == incoming) {
					connectedList.RemoveAt(i);
				}
			}
		}
		print (connectedList.Count);
		//эта функция вызывается извне, отключающейся станцией
	}

	void SetConnectedTo (Transform _connectedTo)
	{
		connectedTo = _connectedTo;
	}

	void SetNullConnectedTo()
	{
		connectedTo = null;
	}

	void Status (bool _connected, Transform _connectedTo)
	{
		//Обновление статуса подключения и станции, к которой подключены
		connected = _connected;
		if (connected) {
			transform.SendMessage ("calcCover");
			//coveringScript.calcCover ();
			DrawConnection (_connectedTo);
		} else {
			transform.SendMessage ("calcUnCover");
			//coveringScript.calcUnCover ();
			deleteLine ();
		}
	}


	//обновление листа
	void UpdateConnectionList()
	{
		if (connectedList.Count > 0) {
			print (connectedList.Count);
			for (int i = 0; i < connectedList.Count; i++) {
				NewCommutation newCommutation = connectedList [i].transform.GetComponent<NewCommutation> ();
				newCommutation.Status (connected, transform);
				if (newCommutation.connectedList.Count > 0) {
					newCommutation.UpdateConnectionList ();
				}
			}
		}
	}
	//ПРОЦЕДУРА ПОДКЛЮЧЕНИЯ
	public void Connect (Transform _hit)
	{
		Transform target = _hit;
		NewCommutation commutation = _hit.transform.GetComponent<NewCommutation> ();
		commutation.AddConnection (transform);
		SetConnectedTo (target);

		if (commutation.connected != connected) {
			Status (commutation.connected, _hit.transform);
			UpdateConnectionList ();
		}

	}


	//ПРОЦЕДУРА ОТКЛЮЧЕНИЯ
	public void DisConnect ()
	{
		deleteLine (); //УДАЛЯЕМ ЛИНИЮ

		if (connectedTo != null) {
			connectedTo.GetComponent<NewCommutation> ().RemoveConnection (transform);
		}

		SetNullConnectedTo (); //ОЧИЩАЕМ К КОМУ ПОДКЛЮЧИЛИСЬ
		Status (false, null);
		UpdateConnectionList ();
	}




	#endregion
	#region Mouse
	void OnMouseEnter()
	{
		OnMouse = true;
	}

	void OnMouseExit()
	{
		OnMouse = false;
	}

	void OnMouseDrag()
	{
		dragging = true;
	}


	#endregion
	#region Start&Update
	void Start () {
		positions = new Vector3[2];
		//if (transform.GetComponent<RayCastCovering> () != null) {
			//coveringScript = transform.GetComponent<RayCastCovering> ();
		//} else if (transform.GetComponent<threeSidesCover> () != null) {
		//	coveringScript = (RayCastCovering)transform.GetComponent<threeSidesCover> ();
		//}
		line = transform.GetComponentInChildren<LineRenderer> ();

	}


	void Update () {
		
		//ОБНОВЛЕНИЕ ЛИНИЙ ПРИ ПЕРЕТАСКИВАНИИ
		if (dragging) {
			if (connectedList.Count > 0) {
				for (int i = 0; i < connectedList.Count; i++) {
					connectedList [i].GetComponent<NewCommutation> ().UpdateLine (transform);
				}
			}
		}
		//ЛКМ ВНЕ НОДЫ
		if (Input.GetButtonDown ("Fire1")) {
			switch (state) 
			{
			case State.Connecting:
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit) && hit.transform.GetComponent<NewCommutation> () != null && hit.transform!=transform) {
					stateConnected (hit.transform);
					SetConnectedTo (hit.transform);
				} else {
					stateDisconnected ();
				}
				break;


			}
		}


		//ПКМ ПО НОДЕ
		if (Input.GetButtonDown ("Fire2") && OnMouse) 
		{
			switch (state) 
			{
			case State.Connected:
				stateDisconnected ();
				stateConnecting ();
				break;
			case State.Connecting:
				stateDisconnected ();
				break;
			case State.Disconnected:
				stateConnecting ();
				break;
			case State.Dragging:
				stateDisconnected ();
				break;
			}
		}

		// ПКМ НЕ ПО НОДЕ
		if (Input.GetButtonDown ("Fire2") && !OnMouse) 
		{
			switch (state) 
			{
			case State.Connecting:
				stateDisconnected ();
				break;
			case State.Dragging:
				setState (prevState);
				break;
			}
		}




	 //ТЕСТ ЛИНИЙ
		/*if (line.enabled == true && connectedTo != null) {
			UpdateLine (connectedTo);
		}
			if (Input.GetButtonDown ("Fire1") && connecting == true) {
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit)) {
					if (hit.transform.GetComponent<NewCommutation> () != null) 
					{
						Connect (hit);
					connecting = false;
					}
				}
			}
		if (Input.GetButtonDown ("Fire2") && OnMouse) 
		{
			connecting = true;
			print ("Connecting On");
		}*/
		}

	#endregion

}