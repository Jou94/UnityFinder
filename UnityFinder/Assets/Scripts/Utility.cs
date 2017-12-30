using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Utility {

	public static T[] ShuffleArray<T>(T[] array) {

		System.Random prng = new System.Random();

		for (int i = 0; i < array.Length - 1; i++){
			int randomIndex = prng.Next(i, array.Length);
			T tempItem = array[randomIndex];
			array[randomIndex] = array[i];
			array[i] = tempItem;
		}

		return array;
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

		public Coord Add (Coord addedCoord) {
			//x += addedCoord.x;
			//y += addedCoord.y;
			return new Coord(x+addedCoord.x,y+addedCoord.y); 
		}

		public int Difference (Coord otherCoord) {
			//Debug.Log(Mathf.Abs(x - otherCoord.x) + Mathf.Abs(y - otherCoord.y));
			return  Mathf.Abs(x - otherCoord.x) + Mathf.Abs(y - otherCoord.y);
		}
	}

	public struct Ini {
		public string name;
		public int initiative;
		public bool isPlayer;

		public Ini (string _name, int _initiative, bool _isPlayer) {
			name = _name;
			initiative = _initiative;
			isPlayer = _isPlayer;
		}
	}
}
