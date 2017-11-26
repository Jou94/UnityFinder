using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	public Transform tilePrefab;
	//public Transform black_background;
	public Vector2 mapSize;

	[Range(0,1)]
	public float outlinePercent;

	void Start() {
		GenerateMap();
	}

	public void GenerateMap() {

		//Vector3 black_background_position = new Vector3 (0,-0.1f,0);
		//black_background = Instantiate(black_background, black_background_position, Quaternion.Euler(Vector3.right*90)) as Transform;
		//black_background.localScale = new Vector3((mapSize.x+1), (mapSize.y+1), 1);

		string holderName = "Generated Map";
		if (transform.FindChild(holderName)) {
			Destroy(transform.FindChild(holderName).gameObject);
		}

		Transform mapHolder = new GameObject (holderName).transform;
		mapHolder.parent = transform;

		for (int x = 0; x < mapSize.x; x++) {
			for (int y = 0; y < mapSize.y; y++) {
				Vector3 tilePosition = new Vector3(-mapSize.x/2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);
				Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right*90)) as Transform;
				newTile.localScale = Vector3.one * (1-outlinePercent);
				newTile.parent = mapHolder;
			}
		}

	}
}
