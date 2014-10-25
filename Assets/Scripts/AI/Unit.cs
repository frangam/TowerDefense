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
	private Node 			originSpawnedCell;
	private Node 			previousTarget;
	private Node 			currentTarget;
	private Node 			finalTarget;
	private List<Node>		path;					//movement path
	private int				currentNodeIndex = 0;
	private float 			currentNodeDistance;	//current target distance to get
	private float 			previousNodeDistance;	//previous target distance to clear previous cell
	private Vector3			targetPos;
	private bool 			noTarget = false;


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

	public int WaveIndex {
		get {
			return this.waveIndex;
		}
	}

	public Node OriginSpawnedCell {
		get {
			return this.originSpawnedCell;
		}
	}

	public Node CurrentTarget {
		get {
			return this.currentTarget;
		}
		set {
			currentTarget = value;
		}
	}

	public Node FinalTarget {
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
			//move to target
			transform.position = Vector3.MoveTowards (transform.position, targetPos, speed * Time.deltaTime);
			previousNodeDistance = Vector3.Distance(transform.position, previousTarget.transform.position);
			currentNodeDistance = Vector3.Distance(transform.position, currentTarget.transform.position);

			//unit gets out from the previous cell
			if(previousNodeDistance > prevNodeDistance){
				previousTarget.GetComponent<Cell>().removeWalkingUnit(this); //clear previous cell target
			}
			//unit get into the current cell
			if(currentNodeDistance < prevNodeDistance){
				currentTarget.GetComponent<Cell>().addWalkingUnit (this); //this unit is walking through this new cell now
			}

			//enemy is enough closer to the next cell
			//get the next target cell
			if(currentNodeDistance < nextNodeDistance){
				previousTarget = currentTarget; //update previous target to current
				setNextTarget();
			}
		}
	}

	/// <summary>
	/// Sets the next target.
	/// </summary>
	private void setNextTarget(){
		if(currentNodeIndex < path.Count){
			Node nextNode = path[currentNodeIndex];
			
			if(nextNode != null){
				CurrentTarget = nextNode; //update current target
				targetPos = new Vector3(CurrentTarget.transform.position.x, transform.position.y, CurrentTarget.transform.position.z);
				currentNodeIndex++;
			}
		}
		//our goal
		else if(CurrentTarget == FinalTarget){
			doSomethingWhenGetTheGoal();
		}
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void init(Node node, int _waveIndex){
		originSpawnedCell = node;
		previousTarget = originSpawnedCell;
		waveIndex = _waveIndex;
		chooseTarget ();
	}
	
	public void applyDamage(float damage){
		life -= damage;

		if(isDead()){
			die();
		}
	}

	public virtual void die(){
		//free previous & current cells
		if(previousTarget != null)
			previousTarget.GetComponent<Cell>().removeWalkingUnit(this);
		if(currentTarget != null)
			currentTarget.GetComponent<Cell>().removeWalkingUnit(this);
		
		Destroy (gameObject);
	}

	public bool isDead(){
		return life <= 0;
	}

	public virtual void doSomethingWhenGetTheGoal(){

	}

	/// <summary>
	/// Chooses a living target. Final Target (destiny) is set by a child class of Unit
	/// Each Unit decides what is your preferred final target
	/// </summary>
	public virtual void chooseTarget(Node _finalTargetNode = null){
		//set the final target
		finalTarget = _finalTargetNode;

		//remove unit from previous and current cells
		if(previousTarget != null)
			previousTarget.GetComponent<Cell>().removeWalkingUnit(this);
		if(currentTarget != null)
			currentTarget.GetComponent<Cell>().removeWalkingUnit(this);
		
		//go back to previous target
		currentTarget = previousTarget;

		//source cell
		Node sourceCell = CurrentTarget == null ? OriginSpawnedCell : CurrentTarget;

		//----
		//get the path
		currentNodeIndex = 0; //restart a new path
		path = PathFinding.BFS (GridGenerator.instance.Grid, sourceCell, FinalTarget, GridGenerator.instance.Width, GridGenerator.instance.Height); 
		noTarget = path.Count == 0;
		setNextTarget (); //get next target cell to go
		//----
	}


	//--------------------------------------
	// Events
	//--------------------------------------
	void onTurretPlaced (Cell cell){
		//if the turret was placed in a cell is in the path of this unit, we need to choose a new path and target
		if(path.Contains(cell))
			chooseTarget();
	}
}
