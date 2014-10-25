using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private TurretType 	type = TurretType.CANNON;
	[SerializeField]
	private float 		damage = 1;
	[SerializeField]
	private float		price = 25;
	[SerializeField]
	private int 		range = 1; 		//unit: number of cells
	[SerializeField]
	private float 		shotRate = 0;	//shot rate in seconds (0 instantly)
	[SerializeField]
	private float 		rotSpeed = 20;
	[SerializeField]
	private Transform	head;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Enemy 		target; 		//enemy to shot
	private List<Enemy> targetsPool;	//pool of enemies

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public Enemy Target {
		get {
			return this.target;
		}
	}
	public float Damage {
		get {
			return this.damage;
		}
	}

	public int Range {
		get {
			return this.range;
		}
	}

	public TurretType Type {
		get {
			return this.type;
		}
	}

	public float Price {
		get {
			return this.price;
		}
		set {
			price = value;
		}
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		targetsPool = new List<Enemy> ();
	}

	public virtual void Update(){
		//change target when current target has died
		if((target != null && target.isDead()) || target == null){
			selectTargetFromPoolAndShot();
		}

		//rotate turret looking at the target
		if(target != null && !target.isDead()){
			rotate();
		}
	}

	void OnTriggerEnter(Collider collider){
		//it is an enemy
		if(collider.tag == Settings.ENEMY_TAG){
			Enemy enemy = collider.GetComponent<Enemy>();

			//add target to our pool
			if(enemy != null && !enemy.isDead())
				targetsPool.Add(enemy);

			//select target when there is not any or when current target has gone
			if(target == null || target.isDead()){
				selectTargetFromPoolAndShot();
			}
		}
	}

	void OnTriggerExit(Collider collider){
		if(collider.tag == Settings.ENEMY_TAG){
			Enemy enemy = collider.GetComponent<Enemy>();

			if(enemy != null){
				//delete unit is out of range from our pool
				if(targetsPool.Contains(enemy))
					targetsPool.Remove(enemy);

				//deselect target and change it
				if(target != null && target == enemy){
					selectTarget(); //deselect
					selectTargetFromPoolAndShot(); //choose other target from pool if exists
				}
			}
		}
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void rotate(){
		Vector3 dir = (target.transform.position - head.position).normalized; //direction from our position to the target position
		Quaternion lookRot = Quaternion.LookRotation(dir); //look at the target
		head.rotation = Quaternion.Slerp(head.rotation, lookRot, Time.deltaTime * rotSpeed); //rotate
	}
	
	private void selectTargetFromPoolAndShot(){
		clearPool (); //first clear the pool

		//choose a valid target
		foreach(Enemy enemy in targetsPool){
			if(enemy != null && !enemy.isDead()){
				selectTarget(enemy); //select a valid target
				StartCoroutine(shotWithInterval());
				break;
			}
		}
	}

	private void clearPool(){
		List<Enemy> unitsToRemove = unitsToRemoveFromPool (); //first get units to remove

		//remove all invalid targets
		foreach(Enemy unit in unitsToRemove){
			targetsPool.Remove(unit);
		}

		//sort the pool with sort enemy criteria (down to top aproach: first enemy that is closer to its crystal than other enemy)
		targetsPool.Sort ();
	}


	private List<Enemy> unitsToRemoveFromPool(){
		List<Enemy> unitsToRemove = new List<Enemy>();
		
		
		foreach(Enemy enemy in targetsPool){
			if(enemy == null || (enemy != null && enemy.isDead())){
				unitsToRemove.Add(enemy);
			}
		}

		return unitsToRemove;
	}




	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Gets a target to shoot
	/// </summary>
	public virtual void selectTarget(Enemy enemy = null){
		target = enemy;
	}

	/// <summary>
	/// Shots to the target
	/// </summary>
	public virtual void shot(){

	}

	public virtual IEnumerator shotWithInterval(){
		while(!GameManager.instance.isGameOver() && target != null && !target.isDead()){
			shot ();
			yield return new WaitForSeconds(shotRate);
		}
	} 
}
