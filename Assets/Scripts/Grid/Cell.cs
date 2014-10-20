using UnityEngine;
using System.Collections;

public class Cell {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int 		x; 						// x coord
	private int 		y; 						// y coord
	private CellType 	type = CellType.NORMAL; //cell type 
	private GameObject	go;						//the gameobject represents this cell


	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public int X {
		get {
			return this.x;
		}
	}

	public int Y {
		get {
			return this.y;
		}
	}

	public CellType Type {
		get {
			return this.type;
		}
	}

	public GameObject Go {
		get {
			return this.go;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public Cell(int _x, int _y, GameObject _go) : this(_x, _y, _go, CellType.NORMAL){}
	public Cell(int _x, int _y,  GameObject _go, CellType _type){
		x = _x;
		y = _y;
		type = _type;
		go = _go;
	}

}
