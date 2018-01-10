using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControllerScript : MonoBehaviour {

	GameObject[,] map;
	bool[,] visitedMap;
	Vector2 mapSize;
	GameObject mapObject;
	MapGenerator mapGeneratorScript;

	TileClass tileclass;

	GameObject player;
	PlayerClass playerScript;
	Utility.Coord playerCoords;
	Transform playerTransform;

	GameObject enemy;
	EnemyClass enemyScript;
	Utility.Coord enemyCoords;
	Transform enemyTransform;

	GameObject cursor;
	CursorController cursorScript;

	GameObject camera;
	CameraController cameraScript;

	int combatantNumber = 0;
	String[] turnOrder;
	List<Utility.Ini> initiatives = new List<Utility.Ini>();
	public int currentTurn = 0;
	bool isPlayerTurn = true;

	Utility.Coord cursorCoords;

	Utility.Coord[] neighbours = {new Utility.Coord (0,1), new Utility.Coord (1,1), new Utility.Coord (1,0), new Utility.Coord (1,-1), new Utility.Coord (0,-1), new Utility.Coord (-1,-1), new Utility.Coord (-1,0), new Utility.Coord (-1,1)};

	// Use this for initialization
	void Start () {
		mapObject = GameObject.Find("Map");
		mapGeneratorScript = mapObject.GetComponent<MapGenerator>();
		map = mapGeneratorScript.getMap();
		mapSize = mapGeneratorScript.getMapSize();
		camera = GameObject.Find("Main Camera");
		cameraScript = camera.GetComponent<CameraController>();
		
	}
	
	// Update is called once per frame
	void Update () {
		CheckTurn();

		if (isPlayerTurn) { 
			if (cursorCoords.Equals(playerCoords)) {
				//Debug.Log("ON THE OBJECTIVE");
				if (Input.GetKeyDown(KeyCode.Alpha1)) {
					Debug.Log("TIME TO MOVE, BOYS!");	
					int playerSpeed = playerScript.getSpeed();
					//(showPlayerMovementArea(playerSpeed/5);
					cursorScript.SetStatePlayerMovement(playerSpeed);
					cursorScript.SetPlayerPosition(playerCoords, playerScript);
					//Debug.Log(playerName);	
				}

				else if (Input.GetKeyDown(KeyCode.Alpha2)) {
					Debug.Log("TIME TO ATTACK, BOYS!");
					int playerRange = playerScript.getRange();
					cursorScript.SetStatePlayerAttack(playerRange);
					cursorScript.SetPlayerPosition(playerCoords, playerScript);	
					cursorScript.SetEnemyPosition(enemyCoords, enemyScript);
				}	
			}
		}
	}

	public Vector2 getMapSize() {return mapSize;}

	public void setMap (GameObject[,] _map, Vector2 _mapSize) {
		map = _map;
		mapSize = _mapSize;
	}

	public void setCursor(GameObject _cursor, CursorController _cursorScript) {
		cursor = _cursor;
		cursorScript = _cursorScript;
	}

	public void setCursorCoords (Utility.Coord _cursorCoords) {cursorCoords = _cursorCoords;}

	public void UpdateCursorStatus (CursorController _cursorScript) {cursorScript = _cursorScript;}

	public Utility.Coord getCursorCoords() {return cursorCoords;}

	public void addPlayer (Utility.Coord _playerCoords, string playerName, Transform _playerTransform) {
		playerCoords = _playerCoords;
		combatantNumber++;

		player = GameObject.Find(playerName);
		playerScript = player.GetComponent<PlayerClass>();
		playerTransform = _playerTransform;
	}

	public void addEnemy (Utility.Coord _enemyCoords, string enemyName, Transform _enemyTransform) {
		enemyCoords = _enemyCoords;
		combatantNumber++;

		enemy = GameObject.Find(enemyName);
		enemyScript = enemy.GetComponent<EnemyClass>();
		enemyTransform = _enemyTransform;


	}

	public void RecieveInitiative (string name, int initiative, bool isPlayer) {
		//Debug.Log("token " + name + " with initiative = " + initiative + " and player = " + isPlayer);
		Utility.Ini newInitiative = new Utility.Ini(name, initiative, isPlayer);
		initiatives.Add(newInitiative);

		initiatives.Sort((p1,p2)=>p1.initiative.CompareTo(p2.initiative));
		initiatives.Reverse();

		for (int i = 0; i < initiatives.Count; i++) {
			Debug.Log ("initiative " + initiatives[i].name + " = " + initiatives[i].initiative);
		}
		
	}

	public void UpdateCursorCoords(int x, int y) {
		if (cursorCoords.x + x > 0 && cursorCoords.x + x < mapSize.x && cursorCoords.y + y > 0 && cursorCoords.y + y < mapSize.y) {
			cursorCoords.x += x;
			cursorCoords.y += y;

		}
		//Debug.Log("Curosr: " + cursorCoords.x + " " + cursorCoords.y + "/ Player: " + playerCoords.x + " " + playerCoords.y);
	}

	public void movePlayer(Utility.Coord newPlayerCoords){
		playerCoords = newPlayerCoords;
		playerTransform.position = mapGeneratorScript.CoordToPosition(playerCoords.x, playerCoords.y);
		cursorScript.SetPlayerPosition(playerCoords, playerScript);
	}

	public void EndTurn() {
		//Debug.Log ("currentTurn = " + currentTurn + " count = " + (initiatives.Count-1));
		if (currentTurn >= (initiatives.Count-1)) {
			//Debug.Log ("currentTurn = " + currentTurn + " count = " + (initiatives.Count-1) + " RESET TO 0");
			currentTurn = 0;
		}
		else {
			//Debug.Log ("NEXT TURN ++");
			currentTurn++;
		}

	}

	private void CheckTurn(){
		if (!initiatives[currentTurn].isPlayer && isPlayerTurn) {
			Debug.Log ("DESTROY CURSOR");
			isPlayerTurn = false;
			cursorScript.Die();
		}
		else if (initiatives[currentTurn].isPlayer && !isPlayerTurn){
			Debug.Log ("INSTANTIATE CURSOR");
			isPlayerTurn = true;
			mapGeneratorScript.InstantiateCursor(playerCoords.x, playerCoords.y); 
			cameraScript.GoTo(playerCoords);

		}
		else if (!initiatives[currentTurn].isPlayer && !isPlayerTurn) {
			cameraScript.GoTo(enemyCoords);
			enemyScript.StartTurn();
		}
	}

	private void showPlayerMovementArea(int movementCells) {
		//Debug.Log("Movement Range = " + movementCells);
		int distance = 0;
		Queue<Utility.Coord> cellQueue = new Queue<Utility.Coord>();
		visitedMap = new bool[(int)mapSize.x,(int)mapSize.y];
		//cellQueue.Enqueue (playerCoords);
		Debug.Log ("Boris is in: " + playerCoords.x + " " + playerCoords.y);

		for (int i = 0; i < 8; i++){
			Debug.Log("volta de bucle: " + i + " cellQueue size: " + cellQueue.Count);
			cellQueue.Enqueue (playerCoords.Add(neighbours[i]));
			bfs(cellQueue, i, distance, movementCells, visitedMap);
		} 
	}

	private void bfs (Queue<Utility.Coord> cellQueue, int i, int distance, int movementCells, bool[,] visitedMap) {
		while (cellQueue.Count > 0 && distance <= movementCells*2) {
			Utility.Coord currentCell = cellQueue.Dequeue();

			
			Debug.Log (currentCell.x + " " + currentCell.y);
			if (currentCell.x >= 0 && currentCell.x < mapSize.x && currentCell.y >= 0 && currentCell.y <  mapSize.y) {
				tileclass = map[currentCell.x,currentCell.y].GetComponent<TileClass>();
				if (tileclass.getType() != 2 && distance <= movementCells) {

					//paint blue
					Debug.Log("Cell " + currentCell.x + " " + currentCell.y + " is painted BLUE");
					//cellQueue.Enqueue(currentCell);

					if (tileclass.getType() == 1) distance +=2;
					else distance++;

					for (int j = 0; j < 8; j++){
						cellQueue.Enqueue(currentCell.Add(neighbours[i]));
						bfs(cellQueue, j, distance, movementCells, visitedMap);
					} 
				}
				else if (tileclass.getType() != 2 && distance <= movementCells*2) {

					//paint yellow
					Debug.Log("Cell " + currentCell.x + " " + currentCell.y + " is painted YELLOW");
					//cellQueue.Enqueue(currentCell);

					if (tileclass.getType() == 1) distance +=2;
					else distance++;

					for (int j = 0; j < 8; j++){
						cellQueue.Enqueue(currentCell.Add(neighbours[i]));
						bfs(cellQueue, j, distance, movementCells, visitedMap);
					} 
				}
			}
		}
	}
}
