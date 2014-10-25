using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding {
	/// http://en.wikipedia.org/wiki/Breadth-first_search#Pseudocode
	public static List<Node> BFS(Node[,] grid, Node root, Node target, int width, int height, bool onlyWalkableNeighbor = true){
		List<Node> path = new List<Node> ();
		Queue<Node> queue = new Queue<Node>();
		HashSet<Node> visitedNodes = new HashSet<Node> ();
		Node[,] prev = new Node[width, height];

		queue.Enqueue (root);
		visitedNodes.Add (root);

		while(queue.Count > 0){
			Node currentNode = queue.Dequeue();

			//if it is target node
			//we get optimal path to go there
			if(currentNode == target){
				path = getPath(prev, root, target);
				break;
			}
			else{
				//visit walkable neigbors
				foreach (Node neighbor in currentNode.Neighbors){
					if ((!onlyWalkableNeighbor || (onlyWalkableNeighbor && neighbor.Walkable)) && !visitedNodes.Contains(neighbor)){
						queue.Enqueue(neighbor);
						visitedNodes.Add(neighbor);
						prev[neighbor.X, neighbor.Y] = currentNode; //set previous node to go from it to the next
					}
				}
			}
		}

		return path;
	}

	/// <summary>
	/// Gets the optimal path
	/// </summary>
	/// <returns>The path.</returns>
	/// <param name="nodes">Nodes.</param>
	/// <param name="source">Source.</param>
	/// <param name="target">Target.</param>
	public static List<Node> getPath( Node[,] nodes, Node source, Node target){
		List<Node> path = new List<Node> ();
		Node nextTarget = target;

		do{
			path.Add( nextTarget );
			nextTarget = nodes[nextTarget.X, nextTarget.Y];
		}
		while(nextTarget != source && nextTarget != null);

		//revert because we get the path inverted from the end node to the starting
		path.Reverse ();

//		//----
//		//For TEST
//		string pathSt= "";
//		for( int i = 0; i < path.Count ; i++){
//			pathSt += "->"+ path[ i ] +"\n";
//		}
//		Debug.Log ("path: " + pathSt);
//		//----

		return path;
	}
	
	/// <summary>
	/// Gets the walkable neighbors.
	/// </summary>
	/// <returns>The walkable neighbors.</returns>
	/// <param name="grid">Grid.</param>
	/// <param name="node">Node.</param>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	public static List<Node> getWalkableNeighbors(Node[,] grid, Node node, int width, int height){
		List<Node> adjacents = new List<Node>();
		
		//horizontal
		for (int i = node.Y - 1; i <= node.Y + 1; i++) {
			if (i > -1 && i < width) {
				Node adj = grid[node.X, i];

				if (adj.Walkable && !adjacents.Contains(adj)) {
					adjacents.Add(adj);
				}
			}
		}

		//vertical
		for (int j = node.X - 1; j <= node.X + 1; j++) {
			if (j > -1 && j < height){
				Node adj = grid[j, node.Y];

				if (adj.Walkable && !adjacents.Contains(adj)) {
					adjacents.Add(adj);
				}
			}
		}

		// Diagonal
		// top left corner
		int x1 = node.X;
		int y1 = node.Y;
		x1--;
		y1--;
		for (int i = x1; i < x1 + 3; i++) {
			if (i > -1 && i < width && y1 > -1 && y1 < height){
				Node adj = grid[i, y1];
				
				if (adj.Walkable && !adjacents.Contains(adj)) {
					adjacents.Add(adj);
				}
			}
			y1++;
		}

		// Diagonal
		// top right corner
		x1 = node.X;
		y1 = node.Y;
		x1--;
		y1++;
		for (int i = x1; i < x1 + 3; i++) {
			if (i > -1 && i < width && y1 > -1 && y1 < height){
				Node adj = grid[i, y1];
				
				if (adj.Walkable && !adjacents.Contains(adj)) {
					adjacents.Add(adj);
				}
			}
			y1--;
		}

		return adjacents;
	}

	/// <summary>
	/// Checks if this node is a Bound
	/// </summary>
	/// <returns><c>true</c>, if bound was ised, <c>false</c> otherwise.</returns>
	/// <param name="node">Node.</param>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	public static bool isBound(Node node, int width, int height){
		return isBound (node.X, node.Y, width, height);
	}

	/// <summary>
	/// Checsk if this coords are a bound
	/// </summary>
	/// <returns><c>true</c>, if bound was ised, <c>false</c> otherwise.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	public static bool isBound(int x, int y, int width, int height){
		bool bound = false;
		
		//horizontal
		for (int i = y - 1; i <= y + 1; i++) {
			bound = i <= -1 || i >= width;
			
			if (bound)
				break;
		}

		if(!bound){
			//vertical
			for (int j = x - 1; j <= x + 1; j++) {
				bound = j <= -1 || j >= height;
				
				if (bound)
					break;
			}
		}
		
		return bound;
	}

}
