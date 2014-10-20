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
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		
	}
	
	void Start(){
		
	}
	
	void FixedUpdate(){
		move (GridGenerator.instance.Grid[GridGenerator.instance.Width-1, GridGenerator.instance.Height-1]);
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	/// <summary>
	/// Move this enemy to the specified target.
	/// </summary>
	/// <param name="target">Target cell</param>
	private void move(Cell target){
		transform.position = Vector3.MoveTowards (transform.position, target.Go.transform.position, speed * Time.deltaTime);
	}

	private void die(){
		if(onDeadUnit != null)
			onDeadUnit(waveIndex);

		Destroy (this);
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(Cell cell, int _waveIndex){
		waveIndex = _waveIndex;
		Vector3 pos = new Vector3(cell.Go.transform.position.x, this.transform.position.y, cell.Go.transform.position.z); //position to locate the enemy
		Instantiate(this, pos, this.transform.rotation); //instantiate enemy to the scene in the correct position
	}

	public void applyDamage(float damage){
		life -= damage;

		//dispatch event if this unit die
		if(isDead() && onDeadUnit != null){

		}
	}

	public bool isDead(){
		return life <= 0;
	}
}
