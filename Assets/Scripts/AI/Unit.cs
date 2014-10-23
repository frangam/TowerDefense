using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float 			initialLife = 100;
	[SerializeField]
	private float 			speed = 2.5f;
	[SerializeField]
	private float 			turnSpeed = 20f;
	[SerializeField]
	private UIProgressBar 	lifeBar; //life progress bar

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int 			waveIndex;
	private float			life;
	private Cell 			originSpawnedCell;
		
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

	public float InitialLife {
		get {
			return this.initialLife;
		}
	}

	public float Speed {
		get {
			return this.speed;
		}
	}

	public float TurnSpeed {
		get {
			return this.turnSpeed;
		}
	}

	public int WaveIndex {
		get {
			return this.waveIndex;
		}
	}

	public Cell OriginSpawnedCell {
		get {
			return this.originSpawnedCell;
		}
	}
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Start(){
		life = initialLife;
		lifeBar.init (initialLife, this.gameObject);
	}

	void Update(){
		lifeBar.Value = life;
	}

	#endregion
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
	public virtual void init(Cell cell, int _waveIndex){
		originSpawnedCell = cell;
		waveIndex = _waveIndex;
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

	public virtual bool MoveToPoint(Vector3 point){
		return false;
		
		float dist=Vector3.Distance(point, transform.position);
		
		//if the unit have reached the point specified
		if(dist<0.15f) return true;
		
		//rotate towards destination
		Quaternion wantedRot=Quaternion.LookRotation(point-transform.position);
		transform.rotation=Quaternion.Slerp(transform.rotation, wantedRot, turnSpeed*Time.deltaTime);
		
		//move, with speed take distance into accrount so the unit wont over shoot
		Vector3 dir=(point-transform.position).normalized;
		transform.Translate(dir*Mathf.Min(dist, speed * Time.deltaTime), Space.World);
		
		return false;
	}
}
