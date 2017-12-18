using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileClass : MonoBehaviour {

	
	public Vector3 pos;
	public int type; //0 = clear, 1 = rough, 2 = wall... (TODO: More types)
	public string ocupiedBy;
	public int coverType; //0 = no cover, 1 = softCover, 2 = fullCover

	//public Tile (/*Vector3 position, int typeTerrain, string character, int cover*/){
		//pos = position;
		//type = typeTerrain;
		//ocupiedBy = character;
		//coverType = cover;
	//}

	public int getType() {return type;}

	public Vector3 getPos() {return pos;}

	public void setType(int Ctype){type = Ctype;}

	public void setPos(Vector3 position){pos = position;}

	

	// Use this for initialization
	void Start () {

		if (gameObject.tag == "DirtTile") {
			//Debug.Log("DirtTerrain");
			pos = transform.position;
			type = 0;
			coverType = 0;

		}

		else if (gameObject.tag == "DirtPlantsTile") {
			//Debug.Log("DirtPlantsTerrain");
			pos = transform.position;
			type = 1;
			coverType = 0;
		}

		else if (gameObject.tag == "DirtRockTile") {
			Debug.Log("DirtRockTerrain");
			pos = transform.position;
			type = 2;
			coverType = 2;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
