using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Unit, System.IComparable<Enemy> {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Cell target;



	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		chooseTarget ();
	}
	
	void OnEnable(){
		CrystalCell.onCaughtCrystal += onCaughtCrystal;
	}

	void OnDisable(){
		CrystalCell.onCaughtCrystal -= onCaughtCrystal;
	}
	
	void FixedUpdate(){
		move ();
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	/// <summary>
	/// Move this enemy to the specified target.
	/// </summary>
	private void move(){
		transform.position = Vector3.MoveTowards (transform.position, target.transform.position, base.Speed * Time.deltaTime);
	}

	/// <summary>
	/// Chooses a living target.
	/// </summary>
	private void chooseTarget(){
		List<Cell> crystalCells = GridGenerator.instance.crystalsCells(); //get living crystal cells
		int cellIndex = Random.Range (0, crystalCells.Count);
		target = crystalCells [cellIndex]; //get the target cell
	}

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
	/// Compares to which enemy is closer to catch its crystal.
	/// 0: if both are the same distance to their crystals
	/// >0: if this is further away to its crystal than other
	/// <0: if this is closer to its crystal than other
	/// </summary>
	/// <returns>Which is closer to catch its crystal</returns>
	/// <param name="other">Other enemy</param>
	public int CompareTo (Enemy other){
		return Vector3.Distance (transform.position, target.transform.position)
			.CompareTo (Vector3.Distance (other.transform.position, other.target.transform.position));
	}

	//--------------------------------------
	// Events
	//--------------------------------------
	void onCaughtCrystal (Cell crystalCell){
		//retarget to a new living crystal cell
		if(!GameManager.instance.isGameOver() && target == crystalCell){
			chooseTarget();
		}
	}


}
