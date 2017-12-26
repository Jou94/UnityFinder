using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControllerScript : MonoBehaviour {

	GameObject[,] map;
	Vector2 mapSize;
	GameObject mapObject;
	MapGenerator mapGeneratorScript;

	GameObject player;
	PlayerClass playerScript;

	int combatantNumber = 0;
	String[] turnOrder;
	List<Utility.Ini> initiatives;

	Utility.Coord cursorCoords;
	Utility.Coord playerCoords;
	Utility.Coord enemyCoords;

	// Use this for initialization
	void Start () {
		mapObject = GameObject.Find("Map");
		mapGeneratorScript = mapObject.GetComponent<MapGenerator>();
		mapSize = mapGeneratorScript.getMapSize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector2 getMapSize() {return mapSize;}

	public void setMap (GameObject[,] _map, Vector2 _mapSize) {
		map = _map;
		mapSize = _mapSize;
	}

	public void setCursorCoords (Utility.Coord _cursorCoords) {cursorCoords = _cursorCoords;}

	public void addPlayer (Utility.Coord _playerCoords, string playerName) {
		playerCoords = _playerCoords;
		combatantNumber++;

		Debug.Log(playerName);

		player = GameObject.Find(playerName);
		playerScript = player.GetComponent<PlayerClass>();
		playerScript.RollInitiative();
	}

	public void addEnemy (Utility.Coord _enemyCoords, string enemyName) {
		enemyCoords = _enemyCoords;
		combatantNumber++;
	}

	public void RecieveInitiative (string name, int initiative, bool isPlayer) {
		Utility.Ini newInitiative = new Utility.Ini(name, initiative, isPlayer);
		initiatives.Add(newInitiative);
		Debug.Log ("Initiative added " + name + " with a " + initiative);
	}

	public void UpdateCursorCoords(int x, int y) {
		cursorCoords.x += x;
		cursorCoords.y += y;
	}
}
