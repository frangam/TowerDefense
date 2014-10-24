﻿using UnityEngine;
using System.Collections;

/// <summary>
/// A utility class
/// </summary>
public class Settings : MonoBehaviour{
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string CELL_TAG = "Cell";
	public const string BOUND_CELL_TAG = "BoundCell";
	public const string CRYSTAL_CELL_TAG = "CrystalCell";
	public const string ENEMY_TAG = "Enemy";
	public const string BULLET_TAG = "Bullet";

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Start () {
		DontDestroyOnLoad (this);
	}
	#endregion
}
