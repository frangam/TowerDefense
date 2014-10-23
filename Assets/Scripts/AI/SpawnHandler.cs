using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnHandler : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Faction 	faction = Faction.ENEMY;		//the faction of this spawner
	[SerializeField]
	private float 		initialDelay = 2f; 				//an initial delay to start to spawn units
	[SerializeField]
	private Wave[] 		waves;							//all of the waves to spawn

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool 		finished = false;				//flag checks if it has finished or not all waves
	private List<Unit>	livingUnits = new List<Unit> ();//living units
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
		livingUnits = new List<Unit> (); //instantiate
		currentWaveIndex = 0;
	}

	void Start(){
		instance.StartCoroutine (instance.spawn ());
	}

	void OnEnable(){
		Wave.onFinishWave += onFinishWave;
		SubWave.onFinishSubWave += onFinishSubWave;
		Unit.onDeadUnit += onDeadUnit;
	}
	
	void OnDisable(){
		Wave.onFinishWave -= onFinishWave;
		SubWave.onFinishSubWave -= onFinishSubWave;
		Unit.onDeadUnit -= onDeadUnit;
	}
	#endregion

	//--------------------------------------
	// Events
	//--------------------------------------
	void onFinishWave (int waveIndex){
		currentWaveIndex++;

		if(currentWaveIndex < waves.Length){
			instance.StartCoroutine(instance.spawn());
		}
	}

	void onFinishSubWave (int waveIndex){
		bool finishWave = instance.waves [waveIndex].checkIfFishish();

		//continue spawningCurrentWave
		if(!finishWave)
			continueSpawningCurrentWave();
	}

	void onDeadUnit (int waveIndex){
		Debug.Log ("Unit is dead of wave" + waveIndex);
//		updateLivingUnits ();
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
		wave.init(currentWaveIndex, faction);
		StartCoroutine(wave.getNextSubWave().spawn(wave.Index));
	}

	private void updateLivingUnits(Unit deadUnit){
		livingUnits.Remove (deadUnit);
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------

	public int livingUnitsNum(){
		return livingUnits.Count;
	}
}
