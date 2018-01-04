using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CursorController : MonoBehaviour {

	private bool Cooldown = false;

	TileClass tileclass;

	GameObject map;
	MapGenerator mapGeneratorScript;

	GameObject combatController;
	CombatControllerScript combatControllerScript;

	GameObject player;
	PlayerClass playerScript;

	GameObject enemy;
	EnemyClass enemyScript;

	public enum GameState {Idle, PlayerMovement, PlayerAttack}
	public GameState currentState;

	public int maxplayerDistance;
	public int playerDistance = 0;
	public int maxAttackRange;
	public int attackRange = 0;

	public string enemyName;

	Vector2 mapSize;
	Utility.Coord playerCoords;
	Utility.Coord cursorCoords;
	Utility.Coord enemyCoords;
	Utility.Coord temporal;
	Vector3 Hoffset = new Vector3 (1,0f,0f);
	Vector3 Voffset = new Vector3 (0f,0f,1);

	// Use this for initialization
	void Start () {

		map = GameObject.Find("Map");
		mapGeneratorScript = map.GetComponent<MapGenerator>();
		mapSize = mapGeneratorScript.getMapSize();

		combatController = GameObject.Find("CombatController");
		combatControllerScript = combatController.GetComponent<CombatControllerScript>();

		cursorCoords = combatControllerScript.getCursorCoords();

		SetStateIdle();
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState){
			case GameState.Idle:
				if (!Cooldown){
					temporal = cursorCoords;
					//Debug.Log(temporal.x + " " + temporal.y);
					//Debug.Log(cursorCoords.x + " " + cursorCoords.y);
					//Debug.Log("Idle");
					//if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) CursorMove("DUpLeft");
					//else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) CursorMove("DUpRight");
					//else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) CursorMove("DDownRight");
					//else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) CursorMove("DDownLeft");
					if (Input.GetKey(KeyCode.A)) CursorMove("Left");
					else if (Input.GetKey(KeyCode.W)) CursorMove("Up");
					else if (Input.GetKey(KeyCode.D)) CursorMove("Right");
					else if (Input.GetKey(KeyCode.S)) CursorMove("Down");
					
					Invoke("ResetCoodldown",0.10f);
					Cooldown = true;
				}
				break;

			case GameState.PlayerMovement:
			//Debug.Log(position.x + " " + position.y);
				temporal = cursorCoords;
				if (!Cooldown){
					//Debug.Log(playerCoords.Difference(cursorCoords.Add(new Utility.Coord(-1,0))));
					//Debug.Log(playerCoords.x + " " + playerCoords.y);
					//Debug.Log(temporal.x + " " + temporal.y);
					//Debug.Log(cursorCoords.x + " " + cursorCoords.y);
					//if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) CursorMove("DUpLeft");
					//else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) CursorMove("DUpRight");
					//else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) CursorMove("DDownRight");
					//else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) CursorMove("DDownLeft");
					(mapGeneratorScript.getTile(cursorCoords.Add(new Utility.Coord(-1,0)))).getType();
					if (Input.GetKey(KeyCode.A) && (mapGeneratorScript.getTile(cursorCoords.Add(new Utility.Coord(-1,0)))).getType() !=2 && playerCoords.Difference(cursorCoords.Add(new Utility.Coord(-1,0))) <= maxplayerDistance) {
						//Debug.Log(playerCoords.Difference(position.Add(new Utility.Coord(-1,0))));
						CursorMove("Left");
						Invoke("ResetCoodldown",0.10f);
						Cooldown = true;
						
					}

					else if (Input.GetKey(KeyCode.W) && (mapGeneratorScript.getTile(cursorCoords.Add(new Utility.Coord(0,1)))).getType() !=2 && playerCoords.Difference(cursorCoords.Add(new Utility.Coord(0,1))) <= maxplayerDistance) {
						//Debug.Log(playerCoords.Difference(position.Add(new Utility.Coord(-1,0))));
						CursorMove("Up");
						Invoke("ResetCoodldown",0.10f);
						Cooldown = true;
						
					}

					else if (Input.GetKey(KeyCode.D) && (mapGeneratorScript.getTile(cursorCoords.Add(new Utility.Coord(1,0)))).getType() !=2 && playerCoords.Difference(cursorCoords.Add(new Utility.Coord(1,0))) <= maxplayerDistance) {
						//Debug.Log(playerCoords.Difference(position.Add(new Utility.Coord(-1,0))));
						CursorMove("Right");
						Invoke("ResetCoodldown",0.10f);
						Cooldown = true;
						
					}

					else if (Input.GetKey(KeyCode.S) && (mapGeneratorScript.getTile(cursorCoords.Add(new Utility.Coord(0,-1)))).getType() !=2 && playerCoords.Difference(cursorCoords.Add(new Utility.Coord(0,-1))) <= maxplayerDistance) {
						//Debug.Log(playerCoords.Difference(position.Add(new Utility.Coord(-1,0))));
						//Debug.Log("MAX IS: "+ maxplayerDistance);
						CursorMove("Down");
						Invoke("ResetCoodldown",0.10f);
						Cooldown = true;
						
					}

				}

				if (Input.GetKeyDown(KeyCode.Alpha1) && !cursorCoords.Equals(playerCoords)) {
					//Debug.Log("Moving here!");
					movePlayer();
					goBack();

				}

				if (Input.GetKey(KeyCode.Q)) goBack();

				break;

			case GameState.PlayerAttack:
				temporal = cursorCoords;
				if (!Cooldown){
					
					//Debug.Log(playerCoords.Difference(temporal.Add(new Utility.Coord(-1,0))));
					//Debug.Log(playerCoords.x + " " + playerCoords.y);
					//Debug.Log(temporal.x + " " + temporal.y);
					//Debug.Log(cursorCoords.x + " " + cursorCoords.y);

					if (Input.GetKey(KeyCode.A) && playerCoords.Difference(cursorCoords.Add(new Utility.Coord(-1,0))) <= maxAttackRange) {
						//Debug.Log(playerCoords.Difference(temporal.Add(new Utility.Coord(-1,0))));
						CursorMove("Left");
						Invoke("ResetCoodldown",0.10f);
						Cooldown = true;
					}

					else if (Input.GetKey(KeyCode.W)  && playerCoords.Difference(cursorCoords.Add(new Utility.Coord(0,1))) <= maxAttackRange) {
						Debug.Log(playerCoords.Difference(temporal.Add(new Utility.Coord(0,1))));
						CursorMove("Up");
						Invoke("ResetCoodldown",0.10f);
						Cooldown = true;
					}

					else if (Input.GetKey(KeyCode.D) && playerCoords.Difference(cursorCoords.Add(new Utility.Coord(1,0))) <= maxAttackRange) {
						//Debug.Log(playerCoords.Difference(temporal.Add(new Utility.Coord(1,0))));
						CursorMove("Right");
						Invoke("ResetCoodldown",0.10f);
						Cooldown = true;
					}

					else if (Input.GetKey(KeyCode.S) && playerCoords.Difference(cursorCoords.Add(new Utility.Coord(0,-1))) <= maxAttackRange) {
						//Debug.Log(playerCoords.Difference(temporal.Add(new Utility.Coord(0,-1))));
						CursorMove("Down");
						Invoke("ResetCoodldown",0.10f);
						Cooldown = true;
					}

				}

				if (Input.GetKeyDown(KeyCode.Alpha2) && cursorCoords.Equals(enemyCoords)) {
					Debug.Log("Attack: 1d8 + bab + strengthMod");
					playerScript.MeleeAttack(enemyScript);
					goBack();

				}

				if (Input.GetKey(KeyCode.Q)) goBack();

				break;
		}
		
	}

	public void SetStateIdle() {
		currentState = GameState.Idle;
	}

	public void SetStatePlayerMovement(int movement) {
		if (currentState == GameState.Idle) { 
			currentState = GameState.PlayerMovement;
			maxplayerDistance = (movement/5)*2;
		}
		
	}

	public void SetStatePlayerAttack(int range) {
		if (currentState == GameState.Idle) { 
			currentState = GameState.PlayerAttack;
			maxAttackRange = range/5;
			//Debug.Log(maxAttackRange);
		}
	}

	public void SetPlayerPosition(Utility.Coord _playerCoords, PlayerClass _playerScript) {
		playerCoords = _playerCoords;
		playerScript = _playerScript;
	}

	public void SetEnemyPosition(Utility.Coord _enemyCoords, EnemyClass _enemyScript) {
		enemyCoords = _enemyCoords;
		enemyScript = _enemyScript;
	}

	private void goBack() {
		playerDistance = 0;
		attackRange = 0;
		currentState = GameState.Idle;
	}

	private void CursorMove(string direction){
		if (direction.Equals("Left") && (transform.position.x - Hoffset.x) >= -Mathf.Floor(mapSize.x/2)) {
			transform.position = transform.position - Hoffset;
			combatControllerScript.UpdateCursorCoords(-1,0);
			cursorCoords = combatControllerScript.getCursorCoords();
			
		}

		else if (direction.Equals("Up") && (transform.position.z + Voffset.z) <= Mathf.Floor(mapSize.y/2)) {
			transform.position = transform.position + Voffset;
			combatControllerScript.UpdateCursorCoords(0,1);
			cursorCoords = combatControllerScript.getCursorCoords();
		}

		else if (direction.Equals("Down") && (transform.position.z - Voffset.z) >= -Mathf.Floor(mapSize.y/2)) {
			transform.position = transform.position - Voffset;
			combatControllerScript.UpdateCursorCoords(0,-1);
			cursorCoords = combatControllerScript.getCursorCoords();
		}

		else if (direction.Equals("Right") && (transform.position.x + Hoffset.x) <= Mathf.Floor(mapSize.x/2)) {
			transform.position = transform.position + Hoffset;
			combatControllerScript.UpdateCursorCoords(1,0);
			cursorCoords = combatControllerScript.getCursorCoords();
		}

		else if (direction.Equals("DUpLeft") && (transform.position.x - Hoffset.x) >= -Mathf.Floor(mapSize.x/2) && (transform.position.z + Voffset.z) <= Mathf.Floor(mapSize.y/2)) {
			transform.position = transform.position - Hoffset;
			transform.position = transform.position + Voffset;
			combatControllerScript.UpdateCursorCoords(-1,1);
			cursorCoords = combatControllerScript.getCursorCoords();
		}

		else if (direction.Equals("DUpRight") && (transform.position.x + Hoffset.x) <= Mathf.Floor(mapSize.x/2) && (transform.position.z + Voffset.z) <= Mathf.Floor(mapSize.y/2)) {
			transform.position = transform.position + Hoffset;
			transform.position = transform.position + Voffset;
			combatControllerScript.UpdateCursorCoords(1,1);
			cursorCoords = combatControllerScript.getCursorCoords();
		}

		else if (direction.Equals("DDownRight") && (transform.position.x + Hoffset.x) <= Mathf.Floor(mapSize.x/2) && (transform.position.z - Voffset.z) >= -Mathf.Floor(mapSize.y/2)) {
			transform.position = transform.position + Hoffset;
			transform.position = transform.position - Voffset;
			combatControllerScript.UpdateCursorCoords(1,-1);
			cursorCoords = combatControllerScript.getCursorCoords();
		}

		else if (direction.Equals("DDownLeft") && (transform.position.x - Hoffset.x) >= -Mathf.Floor(mapSize.x/2) && (transform.position.z - Voffset.z) >= -Mathf.Floor(mapSize.y/2)) {
			transform.position = transform.position - Hoffset;
			transform.position = transform.position - Voffset;
			combatControllerScript.UpdateCursorCoords(-1,-1);
			cursorCoords = combatControllerScript.getCursorCoords();
		}
	}

	private void movePlayer() {
		combatControllerScript.movePlayer(cursorCoords);
	}


	private void ResetCoodldown (){
		Cooldown = false;
	}
}
