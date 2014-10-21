using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Unit {
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
	// Events
	//--------------------------------------
	void onCaughtCrystal (Cell crystalCell){
		Debug.Log ("Retargeting crystal cell");

		//retarget to a new living crystal cell
		if(!GameManager.instance.isGameOver() && target == crystalCell){
			chooseTarget();
		}
	}
}
