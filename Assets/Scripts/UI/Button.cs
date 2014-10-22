using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
public class Button : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Color 	pressedColor = Color.gray;
	[SerializeField]
	private Color 	releasedColor = Color.white;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private GUITexture texture;
	protected bool pressed = false;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		texture = GetComponent<GUITexture> ();
	}
	
	void Update(){
		if (Input.GetMouseButtonDown(0) == true){
			if ( texture.HitTest( Input.mousePosition ) == true){				
				press();
			}
		}
	}

	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void ToggleFx(){
		if (pressed == false){
			texture.color = pressedColor;
		}
		else{
			texture.color = releasedColor;			
		}	

		pressed = !pressed;
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void press(){
		ToggleFx();
	}

	public void ForcePress(){
		pressed = true;
		texture.color = pressedColor;			
	}
	
	public void ForceRelease(){
		pressed = false;
		texture.color = releasedColor;		
	}
	

}
