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
	private string 			finishedGameMessage;

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
		GUI.Label (new Rect (20, 60, 150, 50), "CRYSTALS: " + GameManager.instance.Crystals.ToString ());
		GUI.Label (new Rect (20, 80, 150, 50), "GOLD: " + GameManager.instance.Gold.ToString ());

		//Spawn Button
		GUI.enabled = !spawning;
		if(GUI.Button(new Rect(0,0,100,50), "Spawn")){
		   	spawning = !spawning;
			selectedTurretMessage = "";
			GameManager.instance.startGame(); //start the game
		}

		//Cannon Turret
		GUI.enabled = getTurret (TurretType.CANNON).Price <= GameManager.instance.Gold;
		if(GUI.Button(new Rect(0,100,150,50), "Cannon Turret ("+getTurret(TurretType.CANNON).Price.ToString()+" g.)")){
			selectTurretToPut(TurretType.CANNON);
			selectedTurretMessage = "Cannon Turret selected";
		}

		//Laser Turret
		GUI.enabled = getTurret (TurretType.LASER).Price <= GameManager.instance.Gold;
		if(GUI.Button(new Rect(0,150,150,50), "Laser Turret ("+getTurret(TurretType.LASER).Price.ToString()+" g.)")){
			selectTurretToPut(TurretType.LASER);
			selectedTurretMessage = "Laser Turret selected";
		}

		GUI.Label(new Rect(0, 200, 180, 50), selectedTurretMessage);

		//Restart Game
		GUI.enabled = spawning;
		if(GUI.Button(new Rect(100,0,100,50), "Restart")){
			Application.LoadLevel(0);
		}


		//---
		//Finish Game
		bool go = GameManager.instance.isGameOver ();
		bool win = GameManager.instance.win ();

		finishedGameMessage = "";

		if(go)
			finishedGameMessage = "GAME OVER!!!";
		else if(win)
			finishedGameMessage = "YOU WIN!!!!";

		GUI.enabled = go || win;
		GUI.Label (new Rect (Screen.width/2 - 100, 50, 200, 50), finishedGameMessage);
		//---

		//instructions
		GUI.enabled = true;
		GUI.Label(new Rect (Screen.width/2 - 300, 0, 600, 50), "W,A,S,D: move camera. Right Mouse: rotate camera. Scroll Mouse: Zooming. ESC: Reset view");
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
