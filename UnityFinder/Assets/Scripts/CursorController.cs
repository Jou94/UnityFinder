using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CursorController : MonoBehaviour {

	private bool Cooldown = false;

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
		if (!Cooldown){
			if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) CursorMove("DUpLeft");
			else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) CursorMove("DUpRight");
			else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) CursorMove("DDownRight");
			else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) CursorMove("DDownLeft");
			else if (Input.GetKey(KeyCode.A)) CursorMove("Left");
			else if (Input.GetKey(KeyCode.W)) CursorMove("Up");
			else if (Input.GetKey(KeyCode.D)) CursorMove("Right");
			else if (Input.GetKey(KeyCode.S)) CursorMove("Down");
			
			Invoke("ResetCoodldown",0.15f);
			Cooldown = true;
		}
	}

	void CursorMove(string direction){
		if (direction.Equals("Left") && (transform.position.x - Hoffset.x) >= -Mathf.Floor(mapSize.x/2)) {
			transform.position = transform.position - Hoffset;
			mapGeneratorScript.UpdateCursorCoords(-1,0);
		}

		else if (direction.Equals("Up") && (transform.position.z + Voffset.z) <= Mathf.Floor(mapSize.y/2)) {
			transform.position = transform.position + Voffset;
			mapGeneratorScript.UpdateCursorCoords(0,1);
		}

		else if (direction.Equals("Down") && (transform.position.z - Voffset.z) >= -Mathf.Floor(mapSize.y/2)) {
			transform.position = transform.position - Voffset;
			mapGeneratorScript.UpdateCursorCoords(0,-1);
		}

		else if (direction.Equals("Right") && (transform.position.x + Hoffset.x) <= Mathf.Floor(mapSize.x/2)) {
			transform.position = transform.position + Hoffset;
			mapGeneratorScript.UpdateCursorCoords(1,0);
		}

		else if (direction.Equals("DUpLeft") && (transform.position.x - Hoffset.x) >= -Mathf.Floor(mapSize.x/2) && (transform.position.z + Voffset.z) <= Mathf.Floor(mapSize.y/2)) {
			transform.position = transform.position - Hoffset;
			transform.position = transform.position + Voffset;
			mapGeneratorScript.UpdateCursorCoords(-1,1);
		}

		else if (direction.Equals("DUpRight") && (transform.position.x + Hoffset.x) <= Mathf.Floor(mapSize.x/2) && (transform.position.z + Voffset.z) <= Mathf.Floor(mapSize.y/2)) {
			transform.position = transform.position + Hoffset;
			transform.position = transform.position + Voffset;
			mapGeneratorScript.UpdateCursorCoords(1,1);
		}

		else if (direction.Equals("DDownRight") && (transform.position.x + Hoffset.x) <= Mathf.Floor(mapSize.x/2) && (transform.position.z - Voffset.z) >= -Mathf.Floor(mapSize.y/2)) {
			transform.position = transform.position + Hoffset;
			transform.position = transform.position - Voffset;
			mapGeneratorScript.UpdateCursorCoords(1,-1);
		}

		else if (direction.Equals("DDownLeft") && (transform.position.x - Hoffset.x) >= -Mathf.Floor(mapSize.x/2) && (transform.position.z - Voffset.z) >= -Mathf.Floor(mapSize.y/2)) {
			transform.position = transform.position - Hoffset;
			transform.position = transform.position - Voffset;
			mapGeneratorScript.UpdateCursorCoords(-1,-1);
		}
	}

	void ResetCoodldown (){
		Cooldown = false;
	}
}
