/*
 * Copyright (C) 2014 Francisco Manuel Garcia Moreno
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
