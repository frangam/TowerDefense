using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SubWave {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Unit 			unit;
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
			
			
			unit.init(cell, waveIndex);
			//		livingUnits.Add(unit);
			
			
			
			yield return new WaitForSeconds(rate); //wait
		}
		
		//dispatch event
		if(onFinishSubWave != null)
			onFinishSubWave(waveIndex);

	}
}
