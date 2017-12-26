using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour {

	GameObject combatController;
	CombatControllerScript combatControllerScript;

	string id = "0";
	string name = "Boris";
	string race = "Human";
	int alingment = 4; //1 LG, 2 LN, 3 LE, 4 NG, 5 N, 6 NE, 7 CG, 8 CN, 9 CE
	int size = 0; //0 = medium, -1 = Large, 1 = Small (and so on and so forth)
	int lvl = 1;
	Class[] classes = {new Class("Fighter",1)};
	int strenght = 18;
	int dexterity = 14;
	int constitution = 14;
	int intelligence = 8;
	int wisdom = 8;
	int charisma = 12;
	int strenghtMod;
	int dexterityMod;
	int constitutionMod;
	int intelligenceMod;
	int wisdomMod;
	int charismaMod;
	public int hp;
	int initiative;
	int initiativeMod = 2;
	int speed = 30;
	int armourSpeed = 20;
	int flySpeed = 0;
	int sSeepd = 0;
	int bab = 1;
	public int mAttack;
	int rAttack;
	public int ca;
	int cmb;
	int cmd;
	int ffcmd;
	int bFort = 2;
	int bRefl = 0;
	int bWill = 0;
	int fort;
	int refl;
	int will;
	int dodgeBonus = 0;
	int deflectionBonus = 0;
	int armourBonus = 6;
	int shieldBonus = 0;
	int natArmourBonus = 0;

	// Use this for initialization
	void Start () {

		gameObject.name = this.name;

		combatController = GameObject.Find("CombatController");
		combatControllerScript = combatController.GetComponent<CombatControllerScript>();

		//Debug.Log(classes[0].name+ " " + classes[0].lvl);
		strenghtMod = (int)Mathf.Floor((strenght - 10)/2);
		dexterityMod = (int)Mathf.Floor((dexterity - 10)/2);
		constitutionMod = (int)Mathf.Floor((constitution - 10)/2);
		intelligenceMod = (int)Mathf.Floor((intelligence - 10)/2);
		wisdomMod = (int)Mathf.Floor((wisdom - 10)/2);
		charismaMod = (int)Mathf.Floor((charisma - 10)/2);
		hp = 10 + 1+ constitutionMod;
		mAttack = bab + strenghtMod;
		ca = 10 + armourBonus + dexterityMod + dodgeBonus + deflectionBonus + natArmourBonus + shieldBonus;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public struct Class {
		public string name;
		public int lvl;

		public Class (string _name, int _lvl) {
			name = _name;
			lvl = _lvl;
		}
	}

	public void RollInitiative(){
		Debug.Log("MesageRecieved");
		initiative = Random.Range(0,20) + dexterityMod; //TODO: Add more bonus to initiative roll (feats + misc)
		combatControllerScript.RecieveInitiative(name,initiative,true);
		//Debug.Log(initiative);
	}
}
