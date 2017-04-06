/*
 * Copyright (C) 2014 Francisco Manuel Garcia Moreno
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
	/// Can build if there are not any unit are walking through this cell and it is free
	/// </summary>
	/// <returns><c>true</c>, if build was caned, <c>false</c> otherwise.</returns>
	public bool canBuild(){
		return units.Count == 0;
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
