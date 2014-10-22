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
	private Unit 	target;
	private Vector3 targetPosition;

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(Unit _target, float _damage){
		target = _target;
		targetPosition = target.transform.position;
		damage = _damage;
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Update(){
		if(target != null && followTarget)
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, speed * Time.deltaTime);
		else if(target != null && !followTarget)
			 transform.position += (targetPosition - transform.position) * speed*Time.deltaTime; //Vector3.MoveTowards (transform.position, targetPosition, speed * Time.deltaTime);


	}

	void OnTriggerEnter(Collider collider){

		renderer.enabled = false;

		if(collider.tag == Settings.ENEMY_TAG){
			Enemy enemy = collider.GetComponent<Enemy>();

			//apply damage to the enemy
			if(enemy != null){
				enemy.applyDamage(damage);

			}
		}

		if(collider.tag != Settings.BULLET_TAG){
			renderer.enabled = false;
			Destroy(this.gameObject);
		}
	}


	#endregion
}
