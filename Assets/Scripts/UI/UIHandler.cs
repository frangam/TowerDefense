using UnityEngine;
using System.Collections;

public class UIHandler : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Turret[] 		turrets;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	public Turret 			turretToPut = null;
	private bool 			spawning = false;
	private string 			selectedTurretMessage;

	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public static UIHandler instance;	//singleton



	//--------------------------------------
	// Getters && Setters
	//--------------------------------------
	public bool Spawning {
		get {
			return this.spawning;
		}
	}

	public Turret TurretToPut {
		get {
			return this.turretToPut;
		}
		set {
			turretToPut = value;
		}
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		instance = this;
		spawning = false;
		turretToPut = null;
	}

	void OnGUI(){

		GUI.Label (new Rect (20, 80, 150, 50), "GOLD: " + GameManager.instance.Gold.ToString ());

		//Spawn Button
		GUI.enabled = !spawning;
		if(GUI.Button(new Rect(0,0,100,50), "Spawn")){
		   	spawning = !spawning;
			selectedTurretMessage = "";
			GameManager.instance.startGame(); //start the game
		}

		//Cannon Turret
		if(GUI.Button(new Rect(0,100,150,50), "Cannon Turret ("+getTurret(TurretType.CANNON).Price.ToString()+" g.)")){
			selectTurretToPut(TurretType.CANNON);
			selectedTurretMessage = "Cannon Turret selected";
		}

		//Laser Turret
		if(GUI.Button(new Rect(0,150,150,50), "Laser Turret ("+getTurret(TurretType.CANNON).Price.ToString()+" g.)")){
			selectTurretToPut(TurretType.LASER);
			selectedTurretMessage = "Laser Turret selected";
		}

		GUI.Label(new Rect(0, 200, 180, 50), selectedTurretMessage);

		//Restart Game
		GUI.enabled = spawning;
		if(GUI.Button(new Rect(100,0,100,50), "Restart")){
			Application.LoadLevel(0);
		}
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void selectTurretToPut(TurretType type){
		foreach(Turret t in turrets){
			if(t.Type == type){
				turretToPut = t;
				break;
			}
		}
	}

	private Turret getTurret(TurretType type){
		Turret turret = null;

		foreach(Turret t in turrets){
			if(t.Type == type){
				turret = t;
				break;
			}
		}

		return turret;
	}
}
