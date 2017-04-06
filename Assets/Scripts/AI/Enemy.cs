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

public class Enemy : Unit, System.IComparable<Enemy> {
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public override void OnEnable (){
		base.OnEnable ();
		CrystalCell.onCaughtCrystal += onCaughtCrystal;
	}

	public override void OnDisable (){
		base.OnDisable ();
		CrystalCell.onCaughtCrystal -= onCaughtCrystal;
	}
	#endregion
	

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void catchCrystal(){
		Destroy (gameObject);
	}

	//--------------------------------------
	// Icomparable implementation
	//--------------------------------------
	/// <summary>
	/// Compares which enemy is closer to catch its crystal.
	/// 0: if both are the same distance to their crystals
	/// >0: if this is further away to its crystal than other
	/// <0: if this is closer to its crystal than other
	/// </summary>
	/// <returns>Which is closer to catch its crystal</returns>
	/// <param name="other">Other enemy</param>
	public int CompareTo (Enemy other){
		return Vector3.Distance (transform.position, CurrentTarget.transform.position)
			.CompareTo (Vector3.Distance (other.transform.position, other.CurrentTarget.transform.position));
	}

	//--------------------------------------
	// Redefined Methods
	//--------------------------------------
	/// <summary>
	/// Dos something when get the goal.
	/// </summary>
	public override void doSomethingWhenGetTheGoal (){
		base.doSomethingWhenGetTheGoal ();

		//when an enemy get the goal catchs a crystal
		CrystalCell cc = CurrentTarget.GetComponent<CrystalCell>();
		
		//enemy catchs crystal
		if(cc != null){
			cc.catchCrystal(this);
			Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// Chooses a living target. Final Target (destiny) is set by a child class of Unit
	/// Each Unit decides what is its preferred final target
	/// </summary>
	/// <param name="_finalTargetNode">_final target node.</param>
	public override void chooseTarget (Node _finalTargetNode){
		CrystalCell cc = FinalTarget != null ? FinalTarget.GetComponent<CrystalCell> () : null;
		bool keepFinalTarget = cc != null && !cc.HasCaught;
		List<Cell> crystalCells = null;
		int cellIndex = 0;
		Node finalPreferredTarget = null;

		//dont keep the first final crystal cell, get a new crystal cell target if there is
		if(!keepFinalTarget){
			crystalCells = GridGenerator.instance.crystalsCells(); //get living crystal cells
			
			if(crystalCells.Count > 0){
				cellIndex = Random.Range (0, crystalCells.Count);
				finalPreferredTarget = crystalCells [cellIndex]; //get the final target cell
			}
		}

		//Enemies prefer Crystal Cells like final target
		base.chooseTarget (finalPreferredTarget);
	}

	//--------------------------------------
	// Events
	//--------------------------------------
	/// <summary>
	/// Retarget to a new living crystal cell
	/// </summary>
	/// <param name="crystalCell">Crystal cell.</param>
	/// <param name="enemy">Enemy.</param>
	void onCaughtCrystal (Cell crystalCell, Enemy enemy){
		if(!GameManager.instance.isGameOver() && enemy != this && FinalTarget == crystalCell){
			chooseTarget(crystalCell);
		}
	}


}
