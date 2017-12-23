using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CursorController : MonoBehaviour {

	public GameObject map;
	Vector3 Hoffset = new Vector3 (1,0f,0f);
	Vector3 Voffset = new Vector3 (0f,0f,1);

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) CanItMove("Left");
		else if (Input.GetKeyDown(KeyCode.W)) CanItMove("Up");
		else if (Input.GetKeyDown(KeyCode.S)) CanItMove("Down");
		else if (Input.GetKeyDown(KeyCode.D)) CanItMove("Right");
	}

	void CursorMove(string direction){
		if (direction.Equals("Up")) transform.position = transform.position + Voffset;
		else if (direction.Equals("Down")) transform.position = transform.position - Voffset;
		else if (direction.Equals("Left")) transform.position = transform.position - Hoffset;
		else if (direction.Equals("Right")) transform.position = transform.position + Hoffset;
	}

	void CanItMove(string direction) {
		if (direction.Equals("Up")) gameObject.SendMessage("CanCursorMove","Up");
		else if (direction.Equals("Down")) transform.position = transform.position - Voffset;
		else if (direction.Equals("Left")) transform.position = transform.position - Hoffset;
		else if (direction.Equals("Right")) transform.position = transform.position + Hoffset;
	}
}
