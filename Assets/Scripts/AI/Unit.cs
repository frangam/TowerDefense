using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float 		life = 100;
	[SerializeField]
	private float 		speed = 2.5f;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int 	waveIndex;

	//--------------------------------------
	// Delegates & Events
	//--------------------------------------
	public delegate void dead(int waveIndex);
	public static event dead onDeadUnit;

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public float Life {
		get {
			return this.life;
		}
	}

	public float Speed {
		get {
			return this.speed;
		}
	}

	public int WaveIndex {
		get {
			return this.waveIndex;
		}
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void die(){
		if(onDeadUnit != null)
			onDeadUnit(waveIndex);

		Destroy (gameObject);
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(Cell cell, int _waveIndex){
		waveIndex = _waveIndex;
		Vector3 pos = new Vector3(cell.transform.position.x, this.transform.position.y, cell.transform.position.z); //position to locate the enemy
		Instantiate(this, pos, this.transform.rotation); //instantiate enemy to the scene in the correct position
	}

	public void applyDamage(float damage){
		life -= damage;

		//TODO dispatch event if this unit die
		if(isDead()){
			die();
		}
	}

	public bool isDead(){
		return life <= 0;
	}
}
