using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

	public GameObject map;
	Vector3 Voffset = new Vector3 (0.1f,0f,0f);
	Vector3 Hoffset = new Vector3 (0f,0f,0.1f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey(KeyCode.A)) {
			transform.position = transform.position - Voffset;
			//yield WaitForSeconds(1);
		}

		if (Input.GetKey(KeyCode.W)) {
			transform.position = transform.position + Hoffset;
			//yield WaitForSeconds(1);
		}

		if (Input.GetKey(KeyCode.S)) {
			transform.position = transform.position - Hoffset;
			//yield WaitForSeconds(1);
		}

		if (Input.GetKey(KeyCode.D)) {
			transform.position = transform.position + Voffset;
			//yield WaitForSeconds(1);
		}
	}
}
