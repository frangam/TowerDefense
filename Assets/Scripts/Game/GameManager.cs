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

public class GameManager : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float gold = 300;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int 					crystals = 1;		//number of crystals the player have
	private bool					finishGame = false;	//win or lose
	private bool					startedGame = false;

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public bool StartedGame {
		get {
			return this.startedGame;
		}
	}

	public float Gold {
		get {
			return this.gold;
		}
		set {
			gold = value;
		}
	}

	public int Crystals {
		get {
			return this.crystals;
		}
	}

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
		Time.timeScale = 1;
		startedGame = false;
		finishGame = false;
		crystals = GridGenerator.instance.InitialCrystalsNum;
	}

	void OnEnable(){
		GridGenerator.onStartedWalkableNeighborsNodes += onStartedWalkableNeighborsNodes;
	}
	
	void OnDisable(){
		GridGenerator.onStartedWalkableNeighborsNodes -= onStartedWalkableNeighborsNodes;
	}


	void Update(){
		if(startedGame && !finishGame && isGameOver()){
			finishGame = true;
			pause();

		}
		else if(startedGame && !finishGame && win()){
			finishGame = true;
			pause();
		}
	}
	#endregion


	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void startGame(){
		GridGenerator.instance.UpdateWalkableNeighborsNodes(); //init walkable neigbors nodes
	}

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

	//--------------------------------------
	// Events
	//--------------------------------------
	void onStartedWalkableNeighborsNodes (){
		if(!startedGame)
			startedGame = true; //now it is ready to start the game
	}
}
