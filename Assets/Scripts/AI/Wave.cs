using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Wave {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private SubWave[] 	subwaves = new SubWave[1];	//subwaves with units to spawn in this wave

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool 		finished = false;					//flag checks if it has finished or not this wave
	private List<Unit> 	livingUnits = new List<Unit> ();	//living units
	private int 		index;								//unique id to identify the wave in the collection of waves in the SpawnHandler 
	private Faction		faction = Faction.ENEMY;			
	private int			currentSubWaveIndex = 0;			//current active subwave index			

	//--------------------------------------
	// Delegates & Events
	//--------------------------------------
	public delegate void finishWave(int waveIndex);
	public static event finishWave onFinishWave;

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public int Index {
		get {
			return this.index;
		}
	}


	//--------------------------------------
	// Events
	//--------------------------------------
	void onFinishSubWave (){
		Debug.Log ("Finish Subwave");
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(int _index, Faction _faction){
		index = _index;
		faction = _faction;
	}

	public void checkIfFishish(){
		//dispatch event
		if(currentSubWaveIndex >= subwaves.Length && onFinishWave != null){
			onFinishWave(index);
		}
	}

	public SubWave getNextSubWave(){
		SubWave subwave = null;

		if(!finished){
			subwave = subwaves[currentSubWaveIndex];
			currentSubWaveIndex++;

			if(currentSubWaveIndex >= subwaves.Length)
				finished = true; //update flag
		}

		return subwave;
	}
}
