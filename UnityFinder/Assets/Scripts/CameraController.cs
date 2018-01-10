using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	Vector3 offset = new Vector3 (0,2,0);

	GameObject mapObject;
	MapGenerator mapGeneratorScript;

	// Use this for initialization
	void Start () {
		mapObject = GameObject.Find("Map");
		mapGeneratorScript = mapObject.GetComponent<MapGenerator>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.UpArrow)) transform.position = transform.position + offset;
		if (Input.GetKeyDown(KeyCode.DownArrow)) transform.position = transform.position - offset;
		
	}

	public void GoTo(Utility.Coord coord)	{
		//transform.position = new Vector3 (mapGeneratorScript.CoordXToPosition(coord.x), transform.position.y, mapGeneratorScript.CoordYToPosition(coord.y));
		Vector3 targetPosition = new Vector3 (mapGeneratorScript.CoordXToPosition(coord.x), transform.position.y, mapGeneratorScript.CoordYToPosition(coord.y));
		Vector3 velocity = Vector3.zero;
		float smoothTime = 1.5F;
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
	}
}
