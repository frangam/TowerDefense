using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private int 	damage = 1;
	[SerializeField]
	private int 	range = 1; //unit: number of cells

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Unit 	target; //enemy to shot

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public int Damage {
		get {
			return this.damage;
		}
	}

	public int Range {
		get {
			return this.range;
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

	void Update(){
		//check if there are any enemy in its range to shot it
		target = getTarget ();

		//and shoot to the target
		if(target != null){
			shot();
		}
	}
	#endregion

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Gets a target to shoot
	/// </summary>
	public virtual Unit getTarget(){
		throw new System.NotImplementedException();
	}

	/// <summary>
	/// Shots to the target
	/// </summary>
	public virtual void shot(){

	}
}
