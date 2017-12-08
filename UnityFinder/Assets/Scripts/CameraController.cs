using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject map;
	Vector3 offset = new Vector3 (0,2,0);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.UpArrow)) transform.position = transform.position + offset;
		if (Input.GetKeyDown(KeyCode.DownArrow)) transform.position = transform.position - offset;
		
	}
}
