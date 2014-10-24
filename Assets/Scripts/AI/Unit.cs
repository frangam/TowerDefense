using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	[SerializeField]
	private float 			nextNodeDistance = 0.6f;
	[SerializeField]
	private float 			prevNodeDistance = 0.6f;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int 			waveIndex;
	private float			life;
	private Cell 			originSpawnedCell;
	private Cell 			previousTarget;
	private Cell 			currentTarget;
	private Cell 			finalTarget;
	private List<Node>		path;
	private int				currentNodeIndex = 0;
	private float 			currentNodeDistance;	//current target distance to get
	private float 			previousNodeDistance;	//previous target distance to clear previous cell
	private Vector3			targetPos;
	private bool 			noTarget = false;
		
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

	public Cell CurrentTarget {
		get {
			return this.currentTarget;
		}
		set {
			currentTarget = value;
		}
	}

	public Cell FinalTarget {
		get {
			return this.finalTarget;
		}
		set {
			finalTarget = value;
		}
	}
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		currentNodeIndex = 0;
		noTarget = false;
	}

	public virtual void Start(){
		life = initialLife;
		lifeBar.init (initialLife, this.gameObject);
	}

	public virtual void FixedUpdate(){
		move ();
		
		if(rigidbody.velocity.magnitude == 0 && noTarget){
			chooseTarget();
		}
	}

	public virtual void Update(){
		lifeBar.Value = life;
	}

	public virtual void OnEnable(){
		Cell.onTurretPlaced += onTurretPlaced;
	}
	
	public virtual  void OnDisable(){
		Cell.onTurretPlaced -= onTurretPlaced;
	}

	#endregion
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	/// <summary>
	/// Move this enemy to the specified target.
	/// </summary>
	private  void move(){
		if(CurrentTarget != null){
			transform.position = Vector3.MoveTowards (transform.position, targetPos, speed * Time.deltaTime);
			currentNodeDistance = Vector3.Distance(transform.position, currentTarget.transform.position);
			previousNodeDistance = Vector3.Distance(transform.position, previousTarget.transform.position);


//			Debug.Log("my pos: " + transform.position+ ". tg pos: "+CurrentTarget.transform.position+". dist: " + currentNodeDistance);
			
			//enemy is enough closer to the next cell


			//unit get out from the previous cell
			if(previousNodeDistance > prevNodeDistance){
				previousTarget.removeWalkingUnit(this); //clear previous cell target
			}
			if(currentNodeDistance < prevNodeDistance){
				currentTarget.addWalkingUnit (this); //this unit is walking through this new cell now
			}


			//get the next target cell
			if(currentNodeDistance < nextNodeDistance){
				previousTarget = currentTarget; //update previous target to current
				setNextCell();
			}
		}
	}
	
	private void setNextCell(){
		if(currentNodeIndex < path.Count){
			Cell cell = path[currentNodeIndex].GetComponent<Cell>();
			
			if(cell != null){
				CurrentTarget = cell;
				targetPos = new Vector3(CurrentTarget.transform.position.x, transform.position.y, CurrentTarget.transform.position.z);
				currentNodeIndex++;
			}
		}
		//our goal
		else if(CurrentTarget == FinalTarget){
			doSomethingWhenGetTheGoal();
		}
	}

	private bool cellIsInThePath(Cell cell){
		bool inPath = false;

		foreach(Node n in path){
			inPath = n == cell;

			if(inPath)
				break;
		}

		return inPath;
	}


	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void init(Cell cell, int _waveIndex){
		originSpawnedCell = cell;
		previousTarget = originSpawnedCell;
		waveIndex = _waveIndex;
		chooseTarget ();
	}
	
	public void applyDamage(float damage){
		life -= damage;

		//TODO dispatch event if this unit die
		if(isDead()){
			die();
		}
	}

	public virtual void die(){
		//free current cell
		if(previousTarget != null)
			previousTarget.removeWalkingUnit(this);
		if(currentTarget != null)
			currentTarget.removeWalkingUnit(this);

		if(onDeadUnit != null)
			onDeadUnit(waveIndex);
		
		Destroy (gameObject);
	}

	public bool isDead(){
		return life <= 0;
	}

	public virtual void doSomethingWhenGetTheGoal(){

	}

	//--------------------------------------
	// Protected Methods
	//--------------------------------------
	/// <summary>
	/// Chooses a living target.
	/// </summary>
	protected void chooseTarget(){
		//remove unit
		if(previousTarget != null)
			previousTarget.removeWalkingUnit(this);
		if(currentTarget != null)
			currentTarget.removeWalkingUnit(this);

		//go back to previous target
		currentTarget = previousTarget;

		List<Cell> crystalCells = null;
		int cellIndex = 0;
		bool keepFinalTarget = finalTarget != null && finalTarget.GetComponent<CrystalCell> () != null && !finalTarget.GetComponent<CrystalCell> ().HasCaught;

		//dont keep the first final crystal cell, get a new crystal cell target if there is
		if(!keepFinalTarget){
			crystalCells = GridGenerator.instance.crystalsCells(); //get living crystal cells

			if(crystalCells.Count > 0){
				cellIndex = Random.Range (0, crystalCells.Count);
				FinalTarget = crystalCells [cellIndex]; //get the final target cell
			}
		}

		//source cell
		Cell sourceCell = CurrentTarget == null ? OriginSpawnedCell : CurrentTarget;


		//----
		//get the path
		currentNodeIndex = 0; //restart a new path
		path = PathFinding.BFS (GridGenerator.instance.Grid, sourceCell, FinalTarget, GridGenerator.instance.Width, GridGenerator.instance.Height); 
		noTarget = path.Count == 0;
		setNextCell (); //get next target cell to go
		//----
	}


	//--------------------------------------
	// Events
	//--------------------------------------
	void onTurretPlaced (Cell cell){
		//if the turret was placed in a cell is in the path of this unit, we need to choose a new path and target
		if(cellIsInThePath(cell))
			chooseTarget();
	}
}
