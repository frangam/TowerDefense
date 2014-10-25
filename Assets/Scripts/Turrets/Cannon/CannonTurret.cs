using UnityEngine;
using System.Collections;

public class CannonTurret : Turret {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Transform 	cannonPosition;
	[SerializeField]
	private Bullet 		bulletPb;
	

	//--------------------------------------
	// Redifined Methods
	//--------------------------------------
	public override void shot (){
		Bullet bullet = Instantiate (bulletPb, cannonPosition.position, bulletPb.transform.rotation) as Bullet;
		bullet.init (this.Target, this.Damage, this.Range);
	}


}
