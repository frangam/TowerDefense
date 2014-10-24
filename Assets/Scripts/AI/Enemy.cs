using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Unit, System.IComparable<Enemy> {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private float		range = 0.1f;


	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public override void OnEnable ()
	{
		base.OnEnable ();
		CrystalCell.onCaughtCrystal += onCaughtCrystal;
	}

	public override void OnDisable ()
	{
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
	/// Compares to which enemy is closer to catch its crystal.
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
	public override void doSomethingWhenGetTheGoal ()
	{
		base.doSomethingWhenGetTheGoal ();

		//when an enemy get the goal catchs a crystal
		CrystalCell cc = CurrentTarget.GetComponent<CrystalCell>();
		
		//enemy catchs crystal
		if(cc != null){
			cc.catchCrystal(this);
			Destroy(this.gameObject);
		}
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
			chooseTarget();
		}
	}


}
