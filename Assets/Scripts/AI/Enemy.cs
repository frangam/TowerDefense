using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Unit, System.IComparable<Enemy> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float 		nextNodeDistance = 1.5f;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Cell 		currentTarget;
	private Cell 		finalTarget;
	private float		range = 0.1f;
	private List<Node>	path;
	private int			currentNodeIndex = 0;
	private float 		currentNodeDistance;
	private Vector3		targetPos;
	private bool 		noTarget = false;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		currentNodeIndex = 0;
		noTarget = false;
	}
	
	void OnEnable(){
		CrystalCell.onCaughtCrystal += onCaughtCrystal;
	}

	void OnDisable(){
		CrystalCell.onCaughtCrystal -= onCaughtCrystal;
	}
	
	void FixedUpdate(){
		move ();

		if(rigidbody.velocity.magnitude == 0 && noTarget){
			chooseTarget();
		}
	}
	#endregion
	//--------------------------------------
	// Redefined Methods
	//--------------------------------------
	public override void init (Cell cell, int _waveIndex)
	{
		base.init (cell, _waveIndex);

		chooseTarget ();
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	/// <summary>
	/// Move this enemy to the specified target.
	/// </summary>
	private  void move(){
		if(currentTarget != null){
			transform.position = Vector3.MoveTowards (transform.position, targetPos, base.Speed * Time.deltaTime);
			currentNodeDistance = Vector3.Distance(transform.position, currentTarget.transform.position);

			//enemy is enough closer to the next cell
			if(currentNodeDistance < nextNodeDistance){
				setNextCell();
			}
		}
	}

	private void setNextCell(){
		if(currentNodeIndex < path.Count){
			Cell cell = path[currentNodeIndex].GetComponent<Cell>();

			if(cell != null){
				currentTarget = cell;
				targetPos = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z);
				currentNodeIndex++;
			}
		}
		//our goal
		else if(currentTarget == finalTarget){
			CrystalCell cc = currentTarget.GetComponent<CrystalCell>();

			//enemy catchs crystal
			if(cc != null){
				cc.catchCrystal(this);
				Destroy(this.gameObject);
			}
		}
	}


	/// <summary>
	/// Chooses a living target.
	/// </summary>
	private void chooseTarget(){
		List<Cell> crystalCells = GridGenerator.instance.crystalsCells(); //get living crystal cells
		int cellIndex = Random.Range (0, crystalCells.Count);
		finalTarget = crystalCells [cellIndex]; //get the final target cell
		Cell sourceCell = currentTarget == null ? OriginSpawnedCell : currentTarget; //source cell

		//restart a new path
//		currentTarget = null;
		currentNodeIndex = 0;

		//get the path
		path = PathFinding.BFS (GridGenerator.instance.Grid, sourceCell, finalTarget); 
		noTarget = path.Count == 0;
		setNextCell (); //get next target cell to go
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void catchCrystal(){
		Destroy (gameObject);
	}

	//--------------------------------------
	// Icomparable implementation
	//--------------------------------------
	/// <summary>
	/// Compares to which enemy is closer to catch its crystal.
	/// 0: if both are the same distance to their crystals
	/// >0: if this is further away to its crystal than other
	/// <0: if this is closer to its crystal than other
	/// </summary>
	/// <returns>Which is closer to catch its crystal</returns>
	/// <param name="other">Other enemy</param>
	public int CompareTo (Enemy other){
		return Vector3.Distance (transform.position, currentTarget.transform.position)
			.CompareTo (Vector3.Distance (other.transform.position, other.currentTarget.transform.position));
	}

	//--------------------------------------
	// Events
	//--------------------------------------
	void onCaughtCrystal (Cell crystalCell, Enemy enemy){
		//retarget to a new living crystal cell
		if(!GameManager.instance.isGameOver() && enemy != this && finalTarget == crystalCell){
			chooseTarget();
		}
	}


}
