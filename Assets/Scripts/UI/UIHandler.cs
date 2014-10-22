using UnityEngine;
using System.Collections;

public class UIHandler : MonoBehaviour {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	public TurretButton 	turretBtnPressed;

	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public static UIHandler instance;	//singleton

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		instance = this;
	}
	#endregion

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void updatePressedTurretButton(TurretButton btn = null){
		turretBtnPressed = btn;
	}
}
