﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Generates the world grid.
/// </summary>
public class GridGenerator : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private int 					width = 30; 		//number of cells for the world with
	[SerializeField]
	private int 					height = 30; 		//number of cells for the world height
	[SerializeField]
	private GameObject 				normalCell; 		//basic cell prefab
	[SerializeField]
	private GameObject 				enemyCell; 			//enemy cell prefab
	[SerializeField]
	private GameObject 				crystalCell; 		//crystal cell prefab
	[SerializeField]
	private List<Vector2>			enemiesSpawnerCoords;	//coords of the enemy starting spawn cells
	[SerializeField]
	private List<Vector2>			crystalsCoords;		//coords of the crystal cells

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Cell[,] 				grid;				//all cells of the grid sorted from bottom left to top right, row by row

	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public static GridGenerator		instance; 			//singleton

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public int Width {
		get {
			return this.width;
		}
	}

	public int Height {
		get {
			return this.height;
		}
	}

	public Cell[,] Grid {
		get {
			return this.grid;
		}
	}

	public int CrystalCellsNum {
		get {
			return this.crystalsCoords.Count;
		}
	}

	/// <summary>
	/// Gets the enemies cells.
	/// </summary>
	/// <value>The enemies cells.</value>
	public List<Cell> enemiesCells{
		get{
			List<Cell> cells = new List<Cell>();

			foreach(Vector2 coord in enemiesSpawnerCoords){
				cells.Add(grid[(int) coord.x, (int) coord.y]);
			}

			return cells;
		}
	}

	/// <summary>
	/// Gets the crystals cells.
	/// </summary>
	/// <value>The crystals cells.</value>
	public List<Cell> crystalsCells{
		get{
			List<Cell> cells = new List<Cell>();
			
			foreach(Vector2 coord in crystalsCoords){
				cells.Add(grid[(int) coord.x, (int) coord.y]);
			}
			
			return cells;
		}
	}


	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		instance=this;

		initChecks ();
	}

	void Start(){
		createGrid (); //first, create the grid
		centerGrid (); //then, center it to the screen view
	}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	/// <summary>
	/// Inits the starting checks.
	/// </summary>
	private void initChecks(){
		//check world dimensions
		if(width <= 0 || height <= 0)
			throw new System.ArgumentException("World Dimensions must be greater than zero");
		
		//more cells than allowed
		if(enemiesSpawnerCoords.Count + crystalsCoords.Count > width*height)
			throw new System.ArgumentException("Number of coordinates cannot be greater than world dimension");
		
		
		//------
		//check if there are any repeated coordinate in the setup or if are out of range
		foreach(Vector2 ec in enemiesSpawnerCoords){
			if(ec.x >= width || ec.x < 0 || ec.y >= height || ec.y < 0){
				throw new System.ArgumentOutOfRangeException("Enemy or Crystal Coords cannot be out of world dimensions");
			}
			else if(crystalsCoords.Contains(ec)){
				throw new RepatedCoordsException("Enemy coordinate "+ec+" cannot be the same as a crystal coordinate");
			}
		}
		foreach(Vector2 cc in crystalsCoords){
			if(cc.x >= width || cc.x < 0 || cc.y >= height || cc.y < 0){
				throw new System.ArgumentOutOfRangeException("Enemy or Crystal Coords cannot be out of world dimensions");
			}
		}
		//------

		
		//check must be 1 cell for enemy starting point and 1 cell for crystal cell at least.
		//so we create them if not
		if(enemiesSpawnerCoords.Count == 0){
			enemiesSpawnerCoords.Add(Vector2.zero); //the origin
		}
		if(crystalsCoords.Count == 0){
			crystalsCoords.Add(new Vector2(width-1, height-1));
		}
	}

	/// <summary>
	/// Creates the grid from bottom left to top right, filling row by row.
	/// So this is the indexes distribution:
	/// 
	/// -------------------
	/// |  6  |  7  |  8  |
	/// -------------------
	/// |  3  |  4  |  5  |
	/// -------------------
	/// |  0  |  1  |  2  |
	/// -------------------
	/// </summary>
	private void createGrid(){
		grid = new Cell[width, height]; //instantiate the grid with the corresponding size

		for(int j=0; j<height; j++){
			for(int i=0; i<width; i++){
				GameObject cellAux = normalCell;
				CellType cellType = CellType.NORMAL; //the cell type

				//enemy cell
				if(enemiesSpawnerCoords.Contains(new Vector2(i, j))){
					cellType = CellType.ENEMY_SPAWNER;
					cellAux = enemyCell;
				}
				//crystal cell
				else if(crystalsCoords.Contains(new Vector2(i, j))){
					cellType = CellType.CRYSTAL;
					cellAux = crystalCell;
				}


				//instantiate cell gameobject into the scene
				GameObject cellGO = Instantiate(cellAux) as GameObject;
				cellGO.transform.parent = this.transform;
				cellGO.transform.position = new Vector3(i*normalCell.transform.localScale.x, normalCell.transform.position.y, j*normalCell.transform.localScale.z);

				//create the cell and add it to grid
				Cell cell = new Cell(i, j, cellGO, cellType); 
				grid[i, j] = cell;
			}
		}
	}

	/// <summary>
	/// Centers the grid on the screen
	/// </summary>
	private void centerGrid(){
		transform.position = new Vector3 (-(width*normalCell.transform.localScale.x/2) + Camera.main.transform.position.x + 0.5f, 0, 0);
	}
}
