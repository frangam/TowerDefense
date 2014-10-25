using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private LayerMask 	turretCellsLayer;
	[SerializeField]
	private Material 	selectedCellMat;
	[SerializeField]
	private Material 	basicCellMat;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Cell 		hoverCell;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Update () {
//		if(!UIHandler.instance.Spawning)
			handlePutTurrets();

	}
	#endregion


	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void handlePutTurrets(){
		RaycastHit hit;
		Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
		float x,y,z;
		x=y=z=0f;
		
		
		//hover cells
		if(Physics.Raycast(ray, out hit, 200.0f, turretCellsLayer))
		{
			Debug.DrawLine (Input.mousePosition, hit.point, Color.red, 1);
			Collider cellCollider = hit.collider;

			if(cellCollider.tag == Settings.CELL_TAG){
				Cell cell = cellCollider.GetComponent<Cell>();

				if(cell != hoverCell && cell.canBuild()){
					if(hoverCell != null)
						Materials.changeSharedMaterial(hoverCell.renderer, 1, basicCellMat);

					hoverCell = cell;

					//highlight cell when hover it
					Materials.changeSharedMaterial(hoverCell.renderer, 1, selectedCellMat);
				}


			}
		}
		else{
			if(hoverCell != null)
				Materials.changeSharedMaterial(hoverCell.renderer, 1, basicCellMat);

			hoverCell = null;
		}


		//put turret
		if(Input.GetMouseButtonUp(0) && hoverCell != null && UIHandler.instance.turretToPut != null && hoverCell.isFree()){
			putTurret();
		}
		//clear cell only when game has not started yet
		if(!GameManager.instance.StartedGame && Input.GetMouseButtonUp(1) && hoverCell != null && UIHandler.instance.turretToPut != null && !hoverCell.isFree()){
			clearCell();
		}
	}

	private void putTurret(){
		hoverCell.putTurret (UIHandler.instance.turretToPut);
	}
	private void clearCell(){
		hoverCell.clear ();
	}
}
