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
