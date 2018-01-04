using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour {

	GameObject combatController;
	CombatControllerScript combatControllerScript;

	string id = "50";
	string name = "Nugget";
	string race = "Goblinoid";
	int alingment = 6; //1 LG, 2 LN, 3 LE, 4 NG, 5 N, 6 NE, 7 CG, 8 CN, 9 CE
	int size = 1; //0 = medium, -1 = Large, 1 = Small (and so on and so forth)
	float cr = 1/3;
	//Class[] classes = {new Class("Fighter",1)};
	int strenght = 11;
	int dexterity = 15;
	int constitution = 12;
	int intelligence = 10;
	int wisdom = 9;
	int charisma = 6;
	public int strenghtMod;
	int dexterityMod;
	int constitutionMod;
	int intelligenceMod;
	int wisdomMod;
	int charismaMod;
	public int hp;
	public int initiative;
	int initiativeMod = 6;
	int speed = 30;
	int armourSpeed = 30;
	int flySpeed = 0;
	int sSeepd = 0;
	int bab = 1;
	public int mAttack;
	public int rAttack;
	public int ac;
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
	int armourBonus = 2;
	int shieldBonus = 1;
	int natArmourBonus = 0;

	// Use this for initialization
	void Start () {
		gameObject.name = this.name;
		gameObject.tag = "Enemy";

		combatController = GameObject.Find("CombatController");
		combatControllerScript = combatController.GetComponent<CombatControllerScript>();

		calculateStats();

		RollInitiative();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RollInitiative(){
		initiative = Random.Range(0,20) + dexterityMod; //TODO: Add more bonus to initiative roll (feats + misc)
		combatControllerScript.RecieveInitiative(name,initiative,true);
		//Debug.Log(initiative);
	}

	public bool toHit(int hit) {
		if (hit >= ac) return true;
		return false;
	}

	public void DealDamage (int damage) {
		Debug.Log ("Goblin has been dealt " + damage + " damage.");
		hp -= damage;
		if (hp <= 0) Destroy(gameObject);
	}

	private void calculateStats() {
		strenghtMod = (int)Mathf.Floor((strenght - 10)/2);
		dexterityMod = (int)Mathf.Floor((dexterity - 10)/2);
		constitutionMod = (int)Mathf.Floor((constitution - 10)/2);
		intelligenceMod = (int)Mathf.Floor((intelligence - 10)/2);
		wisdomMod = (int)Mathf.Floor((wisdom - 10)/2);
		charismaMod = (int)Mathf.Floor((charisma - 10)/2);
		hp = 10 + constitutionMod;
		mAttack = bab + strenghtMod + 1;
		rAttack = bab + dexterityMod + 1;
		ac = 10 + armourBonus + dexterityMod + dodgeBonus + deflectionBonus + natArmourBonus + shieldBonus + size;
	}
}
