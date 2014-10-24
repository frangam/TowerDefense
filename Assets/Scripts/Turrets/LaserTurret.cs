using UnityEngine;
using System.Collections;

public class LaserTurret : Turret {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Transform 		cannonPos;
	[SerializeField]
	private LineRenderer 	laserBeamPb;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private LineRenderer 	laserBeam;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	public override void Awake (){
		base.Awake ();
		laserBeam = Instantiate (laserBeamPb) as LineRenderer;
		laserBeam.enabled = false;
	}

	public override void Update(){
		base.Update ();
		laserBeam.enabled = Target != null;
	}

	//--------------------------------------
	// Redifined Methods
	//--------------------------------------
	public override void shot (){
		Vector3 startPos = cannonPos.position; 
		Vector3 finalPos = Target.transform.position;

		laserBeam.SetPosition (0, startPos);
		laserBeam.SetPosition (1, finalPos);

		this.Target.applyDamage (this.Damage);
	}

}
