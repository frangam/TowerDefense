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

public class Bullet : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float 	speed = 5f;
	[SerializeField]
	private bool 	followTarget = false;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private float 	damage;
	private float 	range;
	private Unit 	target;
	private Vector3 targetPosition;
	private float	targetDistance;

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(Unit _target, float _damage, float _range){
		target = _target;
		targetPosition = target.transform.position;
		damage = _damage;
		range = _range;
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void FixedUpdate(){
		if(target != null && followTarget)
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, speed * Time.deltaTime);
		else if(target != null && !followTarget)
			 transform.position += (targetPosition - transform.position) * speed*Time.deltaTime; //Vector3.MoveTowards (transform.position, targetPosition, speed * Time.deltaTime);

		targetDistance += Time.deltaTime * speed;

		if(targetDistance >= range)
			Destroy(this.gameObject);
	}

	void OnTriggerEnter(Collider collider){
		if(collider.tag == Settings.ENEMY_TAG){
			Enemy enemy = collider.GetComponent<Enemy>();

			//apply damage to the enemy
			if(enemy != null){
				enemy.applyDamage(damage);
				Destroy(this.gameObject);
			}
		}
	}


	#endregion
}
