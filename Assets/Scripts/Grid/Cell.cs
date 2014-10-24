using UnityEngine;
using System.Collections;

public class Cell: Node {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private CellType 	type = CellType.NORMAL; //cell type 

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Turret		turret = null;			//if has a turret

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public CellType Type {
		get {
			return this.type;
		}
		set {
			type = value;

			if(tag != null){
				switch(type){
				case CellType.BOUND:
					tag = Settings.BOUND_CELL_TAG;
					break;

				case CellType.NORMAL:
					tag = Settings.CELL_TAG;
					break;

				case CellType.CRYSTAL:
					tag = Settings.CRYSTAL_CELL_TAG;
					break;

				case CellType.ENEMY_SPAWNER:
					tag = Settings.ENEMY_TAG;
					break;
				}
			}
		}
	}
	

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void putTurret(Turret _turret){
		if(type == CellType.NORMAL && isFree() && _turret.Price <= GameManager.instance.Gold){ //if enough money and it is a valid cell
			GameManager.instance.Gold -= _turret.Price; //buy turret

			Vector3 pos = new Vector3 (transform.position.x, _turret.transform.position.y, transform.position.z); 
			turret = Instantiate (_turret, pos, _turret.transform.rotation) as Turret;
//			turret.transform.parent = this.transform;

			Walkable = false;
		}
	}
	public void clear(){
		GameManager.instance.Gold += turret.Price; //restore money
		Destroy (turret.gameObject);
		turret = null;
		Walkable = true;
	}
	public bool isFree(){
		return turret == null;
	}


	//--------------------------------------
	// Redefined Methods
	//--------------------------------------
	public override void init (int _x, int _y, bool _walkable, System.Collections.Generic.List<Node> _neighbors, Node _parent)
	{
		base.init (_x, _y, _walkable, _neighbors, _parent);
	}




}
