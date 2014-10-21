using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int 					crystals = 1;		//number of crystals the player have
	private bool					finishGame = false;	//win or lose

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
		finishGame = false;
		crystals = GridGenerator.instance.InitialCrystalsNum;
		Debug.Log ("Initial Crystals Number: " + crystals);
	}

	void Update(){
		if(!finishGame && isGameOver()){
			finishGame = true;
			Debug.Log("Game Over");
			pause();

		}
		else if(!finishGame && win()){
			finishGame = true;
			Debug.Log("Player Wins");
			pause();
		}
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

	public void pause(bool _pause = true){
		Time.timeScale = _pause ? 0 : 1;
	}

	public void updateCrystalsNum(){
		crystals--;
	}
}
