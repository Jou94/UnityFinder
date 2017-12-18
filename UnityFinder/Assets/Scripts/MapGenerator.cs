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

	List<Coord> allTileCoords;

	Queue<Coord> shuffledTileCoords;

	TileClass tileclass;

	GameObject[,] map;

	Coord cursorPosition;
	Coord playerPosition;
	Coord enemyPosition;

	void Start() {
		GenerateMap();
		InstantiatePlayers();
		InstantiateEnemies();
		InstantiateCursor();
	}

	public void GenerateMap() {

		roughTerrainPercent = Random.Range(0f,1f);
		wallPercent = Random.Range(0f,1f);

		mapSize.x = Random.Range(5,20);
		mapSize.y = Random.Range(5,20);


		//Creating List of tiles
		allTileCoords = new List<Coord> ();

		for (int x = 0; x < mapSize.x; x++) {
			for (int y = 0; y < mapSize.y; y++) {
				allTileCoords.Add(new Coord(x,y));
			}
		}

		shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray()));


		//Populating map with tiles
		string holderName = "Generated Map";
		if (transform.FindChild(holderName)) {
			Destroy(transform.FindChild(holderName).gameObject);
		}

		Transform mapHolder = new GameObject (holderName).transform;
		mapHolder.parent = transform;

		
		map = new GameObject[(int)mapSize.x,(int)mapSize.y]; 

		playerPosition = GetRandomCoord();
		Debug.Log(string.Format("The random coord is {0},{1}", playerPosition.x+1,playerPosition.y+1));

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

			Coord randomCoord = GetRandomCoord();
			wallMap[randomCoord.x, randomCoord.y] = true;
			currentWallCount++;

			//Wall
			if (MapIsFullyAccessible(wallMap,currentWallCount) && !playerPosition.Equals(randomCoord)) {

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

	bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount) {
		bool[,] mapFlags = new bool[obstacleMap.GetLength(0),obstacleMap.GetLength(1)];
		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (playerPosition);
		mapFlags [playerPosition.x, playerPosition.y] = true;

		int accessibleTileCount = 1;

		while (queue.Count > 0) {
			Coord tile = queue.Dequeue();

			for (int x = -1; x <= 1; x ++) {
				for (int y = -1; y <= 1; y ++) {
					int neighbourX = tile.x + x;
					int neighbourY = tile.y + y;
					if (x == 0 || y == 0) {
						if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1)) {
							if (!mapFlags[neighbourX,neighbourY] && !obstacleMap[neighbourX,neighbourY]) {
								mapFlags[neighbourX,neighbourY] = true;
								queue.Enqueue(new Coord(neighbourX,neighbourY));
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

	Vector3 CoordToPosition(int x, int y){
		return new Vector3 (-mapSize.x/2 + 0.5f + x, 0.01f, -mapSize.y/2 + 0.5f + y);
	}

	public Coord GetRandomCoord() {
		Coord randomCoord = shuffledTileCoords.Dequeue();
		shuffledTileCoords.Enqueue (randomCoord);
		return randomCoord;
	}

	public void InstantiatePlayers(){
		bool found = false;
		while (!found){
			playerPosition = GetRandomCoord();
			tileclass = map[playerPosition.x,playerPosition.y].GetComponent<TileClass>();
			if (tileclass.getType() != 2) {
				Player1.localScale = new Vector3(1.8f,1.8f,1.8f); 
				Transform player1 = (Transform)Instantiate(Player1, CoordToPosition(playerPosition.x, playerPosition.y), Quaternion.Euler(Vector3.right*90)) as Transform;
				found = true;
				Debug.Log("Player position is "+playerPosition.x+" "+playerPosition.y);
			}
		}
	}

	public void InstantiateEnemies(){
		bool found = false;
		while (!found){
			enemyPosition = GetRandomCoord();
			tileclass = map[enemyPosition.x,enemyPosition.y].GetComponent<TileClass>();
			if (tileclass.getType() != 2 && !enemyPosition.Equals(playerPosition)) {
				Enemy1.localScale = new Vector3(1.8f,1.8f,1.8f); 
				Transform enemy1 = (Transform)Instantiate(Enemy1, CoordToPosition(enemyPosition.x, enemyPosition.y), Quaternion.Euler(Vector3.right*90)) as Transform;
				found = true;
				Debug.Log("Player position is "+enemyPosition.x+" "+enemyPosition.y);
			}
		}
	}

	public void InstantiateCursor() {
		Transform cursor = (Transform)Instantiate(Cursor, new Vector3(0f,0.5f,0f), Quaternion.Euler(Vector3.right*90)) as Transform;
	}

	public struct Coord {
		public int x;
		public int y;

		public Coord (int _x, int _y) {
			x = _x;
			y = _y;
		}

		public bool Equals (Coord newCoord){
			if (newCoord.x == x && newCoord.y == y) return true;
			return false;
		}
	}
}
