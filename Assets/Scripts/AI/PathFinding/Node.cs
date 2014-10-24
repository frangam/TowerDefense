using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour{
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int 		x;
	private int			y;
	private Node 		parent;
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

	public Node Parent {
		get {
			return this.parent;
		}
		set {
			parent = value;
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
	public virtual void init(int _x, int _y, bool _walkable = true, List<Node> _walkableNeighbors = null, Node _parent = null){
		x = _x;
		y = _y;
		neighbors = _walkableNeighbors;
		walkable = _walkable;
		parent = _parent;
	}

	//--------------------------------------
	// Redefined Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[Node: X={0}, Y={1}, Parent={2}, WalkableNeighbors={3}, Walkable={4}]", X, Y, Parent, Neighbors, Walkable);
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		neighbors = new List<Node> ();
	}
	#endregion

	//--------------------------------------
	// Public Methods
	//--------------------------------------
//	/// <summary>
//	/// Updates the this node on my all of my walkable neighbors.
//	/// </summary>
//	public void updateThisNodeOnMyWalkableNeighbors(bool _walkable = true){
//		walkable = _walkable;
//
//		foreach(Node n in walkableNeighbors)
//			updateWalkableNeighbor(this);
//	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------
//	/// <summary>
//	/// Updates an specific walkable neighbor.
//	/// </summary>
//	/// <param name="node">Node.</param>
//	private void updateWalkableNeighbor(Node node){
//		if(node.walkable && !walkableNeighbors.Contains(node))
//			walkableNeighbors.Add(node);
//		else if(!node.walkable && walkableNeighbors.Contains(node))
//			walkableNeighbors.Remove(node);
//	}
}
