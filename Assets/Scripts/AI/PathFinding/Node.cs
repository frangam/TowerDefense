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
	public virtual void init(int _x, int _y, bool _walkable = true, List<Node> _neighbors = null, Node _parent = null){
		x = _x;
		y = _y;
		neighbors = _neighbors;
		walkable = _walkable;
		parent = _parent;
	}

	//--------------------------------------
	// Redefined Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[Node: X={0}, Y={1}, Parent={2}, Neighbors={3}, Walkable={4}]", X, Y, Parent, Neighbors, Walkable);
	}
}
