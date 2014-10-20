using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int 					crystals = 1;		//number of crystals the player have

	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public static GameManager		instance; 			//singleton

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		instance = this;
		crystals = GridGenerator.instance.CrystalCellsNum;
	}
	#endregion


	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Checks if it is game over or not
	/// </summary>
	/// <returns><c>true</c>, if player has not crystals, <c>false</c> otherwise.</returns>
	public bool isGameOver(){
		return crystals <= 0;
	}


	public bool win(){
		return !isGameOver () && SpawnHandler.instance.Finished && SpawnHandler.instance.livingUnitsNum () <= 0;
	}
}
