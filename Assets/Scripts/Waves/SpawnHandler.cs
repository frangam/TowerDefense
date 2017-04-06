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

public class SpawnHandler : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float 		initialDelay = 2f; 				//an initial delay to start to spawn units
	[SerializeField]
	private Wave[] 		waves;							//all of the waves to spawn

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool 		finished = false;				//flag checks if it has finished or not all waves
	private int			currentWaveIndex = 0;			//current wave index is being spawed

	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public static SpawnHandler		instance; 			//singleton


	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public bool Finished {
		get {
			return this.finished;
		}
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		instance = this;
		finished = false; //init flag
		currentWaveIndex = 0;
	}

	void Start(){
		instance.StartCoroutine (instance.spawn ());
	}

	void OnEnable(){
		Wave.onFinishWave += onFinishWave;
		SubWave.onFinishSubWave += onFinishSubWave;
	}
	
	void OnDisable(){
		Wave.onFinishWave -= onFinishWave;
		SubWave.onFinishSubWave -= onFinishSubWave;
	}
	#endregion

	//--------------------------------------
	// Events
	//--------------------------------------
	void onFinishWave (int waveIndex){
		currentWaveIndex++;
		finished = currentWaveIndex >= waves.Length;

		if(!finished){
			continueSpawningCurrentWave();
		}
	}

	void onFinishSubWave (int waveIndex){
		bool finishWave = instance.waves [waveIndex].checkIfFishish();

		//continue spawningCurrentWave
		if(!finishWave)
			continueSpawningCurrentWave();
	}


	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private IEnumerator spawn(){
		while(!GameManager.instance.StartedGame)
			yield return null;

		yield return new WaitForSeconds(initialDelay); //initial delay


		//only spawn if not game over
		if(!GameManager.instance.isGameOver()){
			continueSpawningCurrentWave();
		}
	}

	private void continueSpawningCurrentWave(){
		Wave wave = waves[currentWaveIndex];
		wave.init(currentWaveIndex);
		StartCoroutine(wave.getNextSubWave().spawn(wave.Index));
	}



	//--------------------------------------
	// Public Methods
	//--------------------------------------

	public int livingUnitsNum(){
		return FindObjectsOfType<Enemy> ().Length;
	}
}
