using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControllerScript : MonoBehaviour {

	GameObject[,] map;
	Vector2 mapSize;
	GameObject mapObject;
	MapGenerator mapGeneratorScript;

	TileClass tileclass;

	GameObject player;
	PlayerClass playerScript;

	int combatantNumber = 0;
	String[] turnOrder;
	List<Utility.Ini> initiatives = new List<Utility.Ini>();

	Utility.Coord cursorCoords;
	Utility.Coord playerCoords;
	Utility.Coord enemyCoords;

	Utility.Coord[] neighbours = {new Utility.Coord (0,1), new Utility.Coord (1,1), new Utility.Coord (1,0), new Utility.Coord (1,-1), new Utility.Coord (0,-1), new Utility.Coord (-1,-1), new Utility.Coord (-1,0), new Utility.Coord (-1,1)};

	// Use this for initialization
	void Start () {
		mapObject = GameObject.Find("Map");
		mapGeneratorScript = mapObject.GetComponent<MapGenerator>();
		map = mapGeneratorScript.getMap();
		mapSize = mapGeneratorScript.getMapSize();


	}
	
	// Update is called once per frame
	void Update () {
		if (cursorCoords.Equals(playerCoords)) {
			//Debug.Log("ON THE OBJECTIVE");
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				//Debug.Log("TIME TO MOVE, BOYS!");	
				int playerSpeed = playerScript.getSpeed();
				showPlayerMovementArea(playerSpeed/5);
				//Debug.Log(playerName);	
			}	
		}
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

		player = GameObject.Find(playerName);
		playerScript = player.GetComponent<PlayerClass>();
	}

	public void addEnemy (Utility.Coord _enemyCoords, string enemyName) {
		enemyCoords = _enemyCoords;
		combatantNumber++;
	}

	public void RecieveInitiative (string name, int initiative, bool isPlayer) {
		Utility.Ini newInitiative = new Utility.Ini(name, initiative, isPlayer);
		initiatives.Add(newInitiative);
		//Debug.Log ("Initiative added " + name + " with a " + initiative);
	}

	public void UpdateCursorCoords(int x, int y) {
		cursorCoords.x += x;
		cursorCoords.y += y;
		//Debug.Log("Curosr: " + cursorCoords.x + " " + cursorCoords.y + "/ Player: " + playerCoords.x + " " + playerCoords.y);
	}

	private void showPlayerMovementArea(int movementCells) {
		//Debug.Log("Movement Range = " + movementCells);
		int distance = 0;
		Queue<Utility.Coord> cellQueue = new Queue<Utility.Coord>();
		cellQueue.Enqueue (playerCoords);
		Debug.Log ("Boris is in: " + playerCoords.x + " " + playerCoords.y);

		for (int i = 0; i < 8; i++){
			bfs(cellQueue, i, distance, movementCells);
		} 
	}

	private void bfs (Queue<Utility.Coord> cellQueue, int i, int distance, int movementCells) {
		while (cellQueue.Count > 0 && distance <= movementCells*2) {
			Utility.Coord currentCell = cellQueue.Dequeue();

			Utility.Coord neighbour = currentCell.Add(neighbours[i]);
			Debug.Log (neighbour.x + " " + neighbour.y);
			if (neighbour.x >= 0 && neighbour.x <= mapSize.x && neighbour.y >= 0 && neighbour.y <=  mapSize.y) {
				tileclass = map[neighbour.x,neighbour.y].GetComponent<TileClass>();
				if (tileclass.getType() != 2 && distance <= movementCells) {

					//paint blue
					Debug.Log("Cell " + neighbour.x + " " + neighbour.y + " is painted BLUE");
					cellQueue.Enqueue(neighbour);

					if (tileclass.getType() == 1) distance +=2;
					else distance++;

					for (int j = 0; j < 8; j++){
						bfs(cellQueue, j, distance, movementCells);
					} 
				}
				else if (tileclass.getType() != 2 && distance <= movementCells*2) {

					//paint yellow
					Debug.Log("Cell " + neighbour.x + " " + neighbour.y + " is painted YELLOW");
					cellQueue.Enqueue(neighbour);

					if (tileclass.getType() == 1) distance +=2;
					else distance++;

					for (int j = 0; j < 8; j++){
						bfs(cellQueue, j, distance, movementCells);
					} 
				}
			}
		}
	}
}
