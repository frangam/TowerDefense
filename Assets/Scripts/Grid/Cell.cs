using UnityEngine;
using System.Collections;

public class Cell: MonoBehaviour {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int 		x; 						// x coord
	private int 		y; 						// y coord
	private CellType 	type = CellType.NORMAL; //cell type 


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

	//--------------------------------------
	// Init Methods
	//--------------------------------------
	public virtual void init(int _x, int _y) {
		init (_x, _y, CellType.NORMAL);
	}
	public virtual void init(int _x, int _y, CellType _type){
		x = _x;
		y = _y;
		type = _type;
	}

}
