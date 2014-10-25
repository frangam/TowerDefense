using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour{
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int 		x;				//x coord in the grid wolrd
	private int			y;				//y coord in the grid wolrd
	private List<Node> 	neighbors;
	private bool 		walkable = true;


	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public int X {
		get {
			return this.x;
		}
	}

	public int Y {
		get {
			return this.y;
		}
	}

	public List<Node> Neighbors {
		get {
			return this.neighbors;
		}
		set {
			neighbors = value;
		}
	}

	public bool Walkable {
		get {
			return this.walkable;
		}
		set {
			walkable = value;
		}
	}

	//--------------------------------------
	// Init Methods
	//--------------------------------------
	public virtual void init(int _x, int _y, bool _walkable = true, List<Node> _walkableNeighbors = null){
		x = _x;
		y = _y;
		neighbors = _walkableNeighbors;
		walkable = _walkable;
	}

	//--------------------------------------
	// Redefined Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[Node: X={0}, Y={1}, WalkableNeighbors={3}, Walkable={4}]", X, Y, Neighbors, Walkable);
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		neighbors = new List<Node> ();
	}
	#endregion
}
