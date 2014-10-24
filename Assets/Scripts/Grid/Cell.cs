using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cell: Node {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private CellType 		type = CellType.NORMAL; //cell type 

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private List<Unit>		units;					//if there are units walking through
	private Turret			turret = null;			//if has a turret

	//--------------------------------------
	// Delegates & Events
	//--------------------------------------
	public delegate void 	turretPlaced(Cell cell);
	public static event 	turretPlaced onTurretPlaced;

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

			Walkable = false;//it is now unwalkable
//			updateThisNodeOnMyWalkableNeighbors(false); //it is now unwalkable


//			GridGenerator.instance.UpdateWalkableNeighborsNodes(); //update walkable neigbors nodes

			//dispatch event
			if(onTurretPlaced != null)
				onTurretPlaced(this);
		}
	}
	public void clear(){
		GameManager.instance.Gold += turret.Price; //restore money
		Destroy (turret.gameObject);
		turret = null;
		Walkable = true;//it is now walkable
//		updateThisNodeOnMyWalkableNeighbors(); //it is now walkable

	}
	public bool isFree(){
		return turret == null && canBuild();
	}

	public void addWalkingUnit(Unit unit){
		if(!units.Contains(unit))
			units.Add (unit);
	}

	public void removeWalkingUnit(Unit unit){
		if(units.Contains(unit))
			units.Remove(unit);
	}

	/// <summary>
	/// Can build if there are not any unit are walking through this cell
	/// </summary>
	/// <returns><c>true</c>, if build was caned, <c>false</c> otherwise.</returns>
	public bool canBuild(){
		return units.Count == 0;
	}

	//--------------------------------------
	// Redefined Methods
	//--------------------------------------
	public override void init (int _x, int _y, bool _walkable, List<Node> _neighbors, Node _parent)
	{
		base.init (_x, _y, _walkable, _neighbors, _parent);
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public override void Awake(){
		base.Awake ();
		units = new List<Unit> ();
	}
	#endregion


}
