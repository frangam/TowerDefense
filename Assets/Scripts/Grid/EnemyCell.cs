using UnityEngine;
using System.Collections;

public class EnemyCell : Cell {
	//--------------------------------------
	// Redefined Methods
	//--------------------------------------
	public override void init (int _x, int _y, bool _walkable, System.Collections.Generic.List<Node> _neighbors, Node _parent){
		base.init (_x, _y, _walkable, _neighbors, _parent);
		base.Type = CellType.ENEMY_SPAWNER;
	}
}
