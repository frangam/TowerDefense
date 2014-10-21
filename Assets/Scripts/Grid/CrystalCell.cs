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
	private int 			crystalQuantity = 1;	//number of available crystals in this cell
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
	public delegate void caughtCrystal(Cell crystalCell);
	public static event caughtCrystal onCaughtCrystal;

	//--------------------------------------
	// Redefined Methods
	//--------------------------------------
	public override void init (int _x, int _y)
	{
		crystalQuantity = Random.Range (minCrystalQuantity, maxCrystalQuantity); //choose crystal quantity
		base.init (_x, _y, CellType.CRYSTAL);


		//instantiate crystal prefabs
		int maxInst = Mathf.Clamp (crystalQuantity, 1, 3);
		for(int i=0; i<maxInst ; i++){
			int indexPb = Random.Range(0, crystalsPbs.Length);
			GameObject cp = crystalsPbs[indexPb]; //get crystal prefab
			Vector3 pos = new Vector3(transform.position.x, cp.transform.position.y, transform.position.z);
			GameObject go = Instantiate(cp, pos, cp.transform.rotation) as GameObject;
			go.transform.parent = this.transform;
		}
	}
	public override void init (int _x, int _y, CellType _type)
	{
		init (_x, _y);
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		hasCaught = false;
	}
	
	void OnCollisionEnter(Collision collision){
		if(!hasCaught && collision.gameObject.tag == Settings.ENEMY_TAG){
			Enemy enemy = collision.gameObject.GetComponent<Enemy>();

			//the enemy catchs the crystal
			if(enemy != null){
				enemy.catchCrystal();
				catchCrystal();
			}
		}
	}
	#endregion


	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void catchCrystal(){
		crystalQuantity--; //update quantity
		GameManager.instance.updateCrystalsNum (); //tell Game Manager update number of crystals

		//has caught all crystals
		if(crystalQuantity <= 0){
			Debug.Log("All crystals have been caught");
			hasCaught = true;
			resetCellToNormalCell();

			if(onCaughtCrystal != null)
				onCaughtCrystal(this);
		}

		//destroy 1 child (crystal gameobject)
		if(crystalQuantity <= transform.childCount){
			foreach(Transform child in transform){
				Destroy(child.gameObject);
				break;
			}
		}
	}

	private void resetCellToNormalCell(){

	}
}
