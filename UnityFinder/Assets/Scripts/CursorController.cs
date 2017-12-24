using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CursorController : MonoBehaviour {

	GameObject map;
	MapGenerator mapGeneratorScript;
	Vector2 mapSize;
	Vector2 initialPos;
	Vector3 Hoffset = new Vector3 (1,0f,0f);
	Vector3 Voffset = new Vector3 (0f,0f,1);

	// Use this for initialization
	void Start () {
		initialPos = transform.position;
		map = GameObject.Find("Map");
		mapGeneratorScript = map.GetComponent<MapGenerator>();
		mapSize = mapGeneratorScript.getMapSize();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) CursorMove("Left");
		else if (Input.GetKeyDown(KeyCode.W)) CursorMove("Up");
		else if (Input.GetKeyDown(KeyCode.S)) CursorMove("Down");
		else if (Input.GetKeyDown(KeyCode.D)) CursorMove("Right");
	}

	void CursorMove(string direction){
		if (direction.Equals("Up") && (transform.position.z + Voffset.z) <= Mathf.Floor(mapSize.y/2)) transform.position = transform.position + Voffset;
		else if (direction.Equals("Down") && (transform.position.z - Voffset.z) >= -Mathf.Floor(mapSize.y/2)) transform.position = transform.position - Voffset;
		else if (direction.Equals("Left") && (transform.position.x - Hoffset.x) >= -Mathf.Floor(mapSize.x/2)) transform.position = transform.position - Hoffset;
		else if (direction.Equals("Right") && (transform.position.x + Hoffset.x) <= Mathf.Floor(mapSize.x/2)) transform.position = transform.position + Hoffset;
	}

	void CanItMove(string direction) {
		if (direction.Equals("Up")) gameObject.SendMessage("CanCursorMove","Up");
		else if (direction.Equals("Down")) transform.position = transform.position - Voffset;
		else if (direction.Equals("Left")) transform.position = transform.position - Hoffset;
		else if (direction.Equals("Right")) transform.position = transform.position + Hoffset;
	}
}
