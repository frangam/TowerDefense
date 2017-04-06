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
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
						Materials.changeSharedMaterial(hoverCell.GetComponent<Renderer>(), 1, basicCellMat);

					hoverCell = cell;

					//highlight cell when hover it
					Materials.changeSharedMaterial(hoverCell.GetComponent<Renderer>(), 1, selectedCellMat);
				}


			}
		}
		else{
			if(hoverCell != null)
				Materials.changeSharedMaterial(hoverCell.GetComponent<Renderer>(), 1, basicCellMat);

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
