using UnityEngine;
using System.Collections;

public class TurretButton : Button {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Turret 		turret;

	//--------------------------------------
	// Redifined Methods
	//--------------------------------------
	public override void press ()
	{
		base.press ();

		if(pressed){
			UIHandler.instance.updatePressedTurretButton(this);
		}
		else{
			UIHandler.instance.updatePressedTurretButton();
		}
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------
}
