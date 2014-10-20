using UnityEngine;
using System.Collections;

public class Enemy : Unit {
	private Cell target;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		int cellIndex = Random.Range (0, GridGenerator.instance.crystalsCells.Count);
		target = GridGenerator.instance.crystalsCells [cellIndex]; //get the target cell
	}
	
	void Start(){
		
	}
	
	void FixedUpdate(){
		move ();
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	/// <summary>
	/// Move this enemy to the specified target.
	/// </summary>
	private void move(){
		transform.position = Vector3.MoveTowards (transform.position, target.Go.transform.position, base.Speed * Time.deltaTime);
	}
}
