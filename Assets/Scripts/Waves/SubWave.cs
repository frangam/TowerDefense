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

[System.Serializable]
public class SubWave {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Unit 			unitPb;
	[SerializeField]
	private int 			quantity = 1;		//quantity of the same unit to spawn
	[SerializeField]
	private float 			rate = 2.3f; 		//spawn frecuency in seconds
	[SerializeField]
	private float 			initialDelay = 2f; 	//an initial delay to start to spawn units

	//--------------------------------------
	// Delegates & Events
	//--------------------------------------
	public delegate void 	finishSubWave(int waveIndex);
	public static event 	finishSubWave onFinishSubWave;

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public IEnumerator spawn(int waveIndex){
		yield return new WaitForSeconds(initialDelay); //initial delay

		List<Cell> unitsCells = GridGenerator.instance.enemiesCells(); //cells where units are spawned (default at enemies cells)

		for(int i=0; i<quantity; i++){
			//only spawn if not game over
			if(GameManager.instance.isGameOver())
				break;
			
			//choose cell where we are going to spawn the enemy randomly
			int cellIndex = Random.Range(0, unitsCells.Count); 
			Cell cell = unitsCells[cellIndex]; //get chosen cell
			

			//instantiate unit
			Vector3 pos = new Vector3(cell.transform.position.x, unitPb.transform.position.y, cell.transform.position.z); //position to locate the enemy
			Unit unit = MonoBehaviour.Instantiate(unitPb, pos, unitPb.transform.rotation) as Unit;
			unit.init(cell, waveIndex); //init the unit			
			
			
			yield return new WaitForSeconds(rate); //wait
		}
		
		//dispatch event
		if(onFinishSubWave != null)
			onFinishSubWave(waveIndex);

	}
}
