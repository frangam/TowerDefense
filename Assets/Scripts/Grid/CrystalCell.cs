using UnityEngine;
using System.Collections;

public class CrystalCell : Cell {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private GameObject[] 	crystalsPbs;			//crystals prefabs available
	[SerializeField]
	private int 			minCrystalQuantity = 1;	//minimum number of available crystals in this cell
	[SerializeField]
	private int 			maxCrystalQuantity = 1;	//maximum number of available crystals in this cell

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	public int 			crystalQuantity = 1;	//number of available crystals in this cell
	private bool 			hasCaught = false;

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public int CrystalQuantity {
		get {
			return this.crystalQuantity;
		}
	}
	public bool HasCaught {
		get {
			return this.hasCaught;
		}
	}

	//--------------------------------------
	// Delegates & Events
	//--------------------------------------
	public delegate void caughtCrystal(Cell crystalCell, Enemy enemy);
	public static event caughtCrystal onCaughtCrystal;

	//--------------------------------------
	// Redefined Methods
	//--------------------------------------
	public override void init (int _x, int _y, bool _walkable, System.Collections.Generic.List<Node> _neighbors, Node _parent){
		base.init (_x, _y, _walkable, _neighbors, _parent);
		base.Type = CellType.CRYSTAL;

		//choose crystal quantity
		crystalQuantity = Random.Range (minCrystalQuantity, maxCrystalQuantity); 

		//instantiate crystal prefabs
		int maxInst = Mathf.Clamp (crystalQuantity, 1, 3);
		for(int i=0; i<maxInst ; i++){
			int indexPb = Random.Range(0, crystalsPbs.Length);
			GameObject cp = crystalsPbs[indexPb]; //get crystal prefab
			GameObject go = Instantiate(cp, transform.position, cp.transform.rotation) as GameObject;
			go.transform.parent = this.transform;
		}
	}


	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public override void Awake(){
		base.Awake ();
		hasCaught = false;
	}
	#endregion


	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void catchCrystal(Enemy enemy){
		crystalQuantity--; //update quantity
		GameManager.instance.updateCrystalsNum (); //tell Game Manager update number of crystals

		//has caught all crystals
		if(crystalQuantity <= 0){
			hasCaught = true;

			if(onCaughtCrystal != null)
				onCaughtCrystal(this, enemy);
		}

		//destroy 1 child (crystal gameobject)
		if(crystalQuantity < transform.childCount){
			foreach(Transform child in transform){
				Destroy(child.gameObject);
				break;
			}
		}
	}


}
