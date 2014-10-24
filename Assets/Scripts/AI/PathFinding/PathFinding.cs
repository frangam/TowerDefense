using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding {
	/// http://en.wikipedia.org/wiki/Breadth-first_search#Pseudocode
	public static List<Node> BFS(Node[,] grid, Node root, Node target){
		List<Node> path = new List<Node> ();
		Queue<Node> queue = new Queue<Node>();
		HashSet<Node> visitedNodes = new HashSet<Node> ();

		Node[,] prev = new Node[GridGenerator.instance.Width, GridGenerator.instance.Height];

		queue.Enqueue (root);
		visitedNodes.Add (root);

		while(queue.Count > 0){
			Node node = queue.Dequeue();

			//if it is target node
			//we get optimal path to go there
			if(node == target){
				path = getPath(prev, root, target);
				break;
			}
			else{
				//visit neigbors
				foreach (Node neighbor in node.Neighbors){
					if (!visitedNodes.Contains(neighbor)){
						queue.Enqueue(neighbor);
						visitedNodes.Add(neighbor);
						prev[neighbor.X, neighbor.Y] = node;
					}
				}
			}
		}

		return path;
	}

	public static List<Node> getPath( Node[,] nodes, Node source, Node target){
		List<Node> path = new List<Node> ();

		Node nextTarget = target;

		 do{
			path.Add( nextTarget );

			nextTarget = nodes[nextTarget.X, nextTarget.Y];

//			target = nodes[ target.X, target.Y ];
		}
		while(nextTarget != source);
//		while(nodes[target.X, target.Y] != source);

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
	

	public static List<Node> walkableNeighbors(Node[,] grid, Node node){
		List<Node> adjacents = new List<Node>();
		
		//horizontal
		for (int i = node.Y - 1; i <= node.Y + 1; i++) {
			if (i > -1 && i < GridGenerator.instance.Width) {
				Node adj = grid[node.X, i];

				if (adj.Walkable) {
					adjacents.Add(adj);
				}
			}
		}

		//vertical
		for (int j = node.X - 1; j <= node.X + 1; j++) {
			if (j > -1 && j < GridGenerator.instance.Height){
				Node adj = grid[j, node.Y];

				if (adj.Walkable) {
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
			if (i > -1 && i < GridGenerator.instance.Width && y1 > -1 && y1 < GridGenerator.instance.Height){
				Node adj = grid[i, y1];
				
				if (adj.Walkable) {
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
			if (i > -1 && i < GridGenerator.instance.Width && y1 > -1 && y1 < GridGenerator.instance.Height){
				Node adj = grid[i, y1];
				
				if (adj.Walkable) {
					adjacents.Add(adj);
				}
			}
			y1--;
		}

		return adjacents;
	}
}
