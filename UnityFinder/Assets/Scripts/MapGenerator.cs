using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapGenerator : MonoBehaviour {

	public Transform Dirt1Tile;
	public Transform DirtPlants1Tile;
	public Transform DirtRock1Tile;
	public Vector2 mapSize;
	public Transform Cursor;
	public Transform Player1;
	public Transform Enemy1;
	

	[Range(0,1)]
	public float outlinePercent;

	[Range(0,1)]
	public float roughTerrainPercent;

	[Range(0,1)]
	public float wallPercent;

	List<Utility.Coord> allTileCoords;

	Queue<Utility.Coord> shuffledTileCoords;

	TileClass tileclass;

	GameObject[,] map;

	GameObject combatController;
	CombatControllerScript combatControllerScript;

	GameObject cursor;
	CursorController cursorScript;

	Utility.Coord cursorCoords;
	Utility.Coord playerCoords;
	Utility.Coord enemyCoords;

	void Start() {

		combatController = GameObject.Find("CombatController");
		combatControllerScript = combatController.GetComponent<CombatControllerScript>();
		combatControllerScript.setMap(map, mapSize);

		GenerateMap();
		InstantiatePlayers();
		InstantiateEnemies();
		InstantiateCursor();

		cursor = GameObject.Find("Cursor(Clone)");
		cursorScript = cursor.GetComponent<CursorController>();

		combatControllerScript.setCursor(cursor, cursorScript);
	}

	void Update() {

	}

	public Vector2 getMapSize() {return mapSize;}

	public GameObject[,] getMap() {return map;}

	public Vector3 CoordToPosition(int x, int y){
		return new Vector3 (-mapSize.x/2 + 0.5f + x, 0.01f, -mapSize.y/2 + 0.5f + y);
	}

	public float CoordXToPosition(int x){
		return -mapSize.x/2 + 0.5f + x;
	}

	public float CoordYToPosition(int y){
		return -mapSize.y/2 + 0.5f + y;
	}

	public TileClass getTile (Utility.Coord coord) {
		return map[coord.x, coord.y].GetComponent<TileClass>();
	}

	public Utility.Coord GetRandomCoord() {
		Utility.Coord randomCoord = shuffledTileCoords.Dequeue();
		shuffledTileCoords.Enqueue (randomCoord);
		return randomCoord;
	}

	private void GenerateMap() {

		roughTerrainPercent = Random.Range(0f,1f);
		wallPercent = Random.Range(0f,1f);

		mapSize.x = Random.Range(10,20);
		mapSize.y = Random.Range(10,20);


		//Creating List of tiles
		allTileCoords = new List<Utility.Coord> ();

		for (int x = 0; x < mapSize.x; x++) {
			for (int y = 0; y < mapSize.y; y++) {
				allTileCoords.Add(new Utility.Coord(x,y));
			}
		}

		shuffledTileCoords = new Queue<Utility.Coord>(Utility.ShuffleArray(allTileCoords.ToArray()));


		//Populating map with tiles
		string holderName = "Generated Map";
		if (transform.FindChild(holderName)) {
			Destroy(transform.FindChild(holderName).gameObject);
		}

		Transform mapHolder = new GameObject (holderName).transform;
		mapHolder.parent = transform;

		
		map = new GameObject[(int)mapSize.x,(int)mapSize.y]; 

		playerCoords = GetRandomCoord();

		for (int x = 0; x < mapSize.x; x++) {
			for (int y = 0; y < mapSize.y; y++) {

				Vector3 tilePosition = new Vector3(-mapSize.x/2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);

				//Rough Terrain
				if (Random.value < roughTerrainPercent) {
					
					Transform newTile = (Transform)Instantiate(DirtPlants1Tile, tilePosition, Quaternion.Euler(Vector3.right*90));
					newTile.localScale = new Vector3(5,5,5) * (1-outlinePercent);
					newTile.parent = mapHolder;
					GameObject tile = newTile.gameObject;
					map[x,y] = tile;
					tileclass = map[x,y].GetComponent<TileClass>();
					tileclass.setType(1);
					tileclass.setPos(tilePosition);
					//Debug.Log(tileclass.getPos());

				}

				//Basic Terrain
				else {
					
					Transform newTile = (Transform)Instantiate(Dirt1Tile, tilePosition, Quaternion.Euler(Vector3.right*90));
					newTile.localScale = new Vector3(5,5,5) * (1-outlinePercent);
					newTile.parent = mapHolder;
					GameObject tile = newTile.gameObject;
					map[x,y] = tile;
					tileclass = map[x,y].GetComponent<TileClass>();
					tileclass.setType(0);
					tileclass.setPos(tilePosition);
					//Debug.Log(tileclass.getType());

				}

			}
		}

		//Walls
		bool[,] wallMap = new bool[(int)mapSize.x,(int)mapSize.y];

		int wallCount = (int)(mapSize.x * mapSize.y * wallPercent);
		int currentWallCount = 0;

		for (int i = 0; i < wallCount; i ++) {

			Utility.Coord randomCoord = GetRandomCoord();
			wallMap[randomCoord.x, randomCoord.y] = true;
			currentWallCount++;

			//Wall
			if (MapIsFullyAccessible(wallMap,currentWallCount) && !playerCoords.Equals(randomCoord)) {

				Vector3 wallPosition = CoordToPosition(randomCoord.x, randomCoord.y);
				wallPosition = wallPosition + new Vector3 (0,0.001f,0);

				Transform newTile = (Transform)Instantiate(DirtRock1Tile, wallPosition, Quaternion.Euler(Vector3.right*90));
				newTile.localScale = new Vector3(5,5,5) * (1-outlinePercent);
				newTile.parent = mapHolder;
				GameObject tile = newTile.gameObject;
				map[randomCoord.x,randomCoord.y] = tile;
				tileclass = tile.GetComponent<TileClass>();
				tileclass.setType(2);
				tileclass.setPos(wallPosition);
				//Debug.Log(tileclass.coverType);
				wallMap[randomCoord.x, randomCoord.y] = true;
			}
			else {
				wallMap[randomCoord.x, randomCoord.y] = false;
				currentWallCount--;
			}
		}
	}

	private bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount) {
		bool[,] mapFlags = new bool[obstacleMap.GetLength(0),obstacleMap.GetLength(1)];
		Queue<Utility.Coord> queue = new Queue<Utility.Coord> ();
		queue.Enqueue (playerCoords);
		mapFlags [playerCoords.x, playerCoords.y] = true;

		int accessibleTileCount = 1;

		while (queue.Count > 0) {
			Utility.Coord tile = queue.Dequeue();

			for (int x = -1; x <= 1; x ++) {
				for (int y = -1; y <= 1; y ++) {
					int neighbourX = tile.x + x;
					int neighbourY = tile.y + y;
					if (x == 0 || y == 0) {
						if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1)) {
							if (!mapFlags[neighbourX,neighbourY] && !obstacleMap[neighbourX,neighbourY]) {
								mapFlags[neighbourX,neighbourY] = true;
								queue.Enqueue(new Utility.Coord(neighbourX,neighbourY));
								accessibleTileCount ++;
							}
						}
					}
				}
			}
		}
		int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
		return targetAccessibleTileCount == accessibleTileCount;
	}

	private void InstantiatePlayers(){
		bool found = false;
		while (!found){
			playerCoords= GetRandomCoord();
			tileclass = map[playerCoords.x,playerCoords.y].GetComponent<TileClass>();
			if (tileclass.getType() != 2) {
				Player1.localScale = new Vector3(1.8f,1.8f,1.8f); 
				//Player1.name = "Cyka";
				//Player1.tag = "Player";
				Transform player1 = (Transform)Instantiate(Player1, CoordToPosition(playerCoords.x, playerCoords.y), Quaternion.Euler(Vector3.right*90)) as Transform;
				found = true;
				combatControllerScript.addPlayer(playerCoords, player1.name, player1);
				//Debug.Log("Player position is "+playerCoords.x+" "+playerCoords.y);
			}
		}
	}

	private void InstantiateEnemies(){
		bool found = false;
		while (!found){
			enemyCoords = GetRandomCoord();
			tileclass = map[enemyCoords.x,enemyCoords.y].GetComponent<TileClass>();
			if (tileclass.getType() != 2 && !enemyCoords.Equals(playerCoords)) {
				Enemy1.localScale = new Vector3(1.8f,1.8f,1.8f); 
				Transform enemy1 = (Transform)Instantiate(Enemy1, CoordToPosition(enemyCoords.x, enemyCoords.y), Quaternion.Euler(Vector3.right*90)) as Transform;
				found = true;
				combatControllerScript.addEnemy(enemyCoords, enemy1.name, enemy1);
				//Debug.Log("Enemy position is "+enemyCoords.x+" "+enemyCoords.y);
			}
		}
	}

	private void InstantiateCursor() {
		cursorCoords = new Utility.Coord ((int)mapSize.x/2, (int)mapSize.y/2);
		Transform cursor = (Transform)Instantiate(Cursor, new Vector3 (CoordXToPosition((int)mapSize.x/2),0.05f,CoordYToPosition((int)mapSize.y/2)), Quaternion.Euler(Vector3.right*90)) as Transform;
		combatControllerScript.setCursorCoords(cursorCoords);
		//Debug.Log("Cursor position is "+cursorCoords.x+" "+cursorCoords.y);
	}
}
