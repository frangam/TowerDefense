using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraHandler : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool 		canMove = true;
	[SerializeField]
	private bool		canRotate = true;
	[SerializeField]
	private bool		canZoom = true;
	[SerializeField]
	private float 		minPosX = - 10;
	[SerializeField]
	private float 		maxPosX = 10;
	[SerializeField]
	private float 		minPosZ = -10;
	[SerializeField]
	private float 		maxPosZ = 10;
	[SerializeField]
	private float 		minRotAngle = 10;	//minimum angle for rotation
	[SerializeField]
	private float 		maxRotAngle = 90;	//maximum angle for rotation
	[SerializeField]
	private float 		minZoom = 8;
	[SerializeField]
	private float 		maxZoom = 30;
	[SerializeField]
	private float 		movementSpeed = 5;
	[SerializeField]
	private float 		zoomSpeed = 3;
	[SerializeField]
	private LayerMask 	zoomMask;
	[SerializeField]
	private Transform 	refTransformZoom;	//reference gameobject of the grid for zoom

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Vector3 	initialPosition; 	//the initial position
	private Quaternion 	initialRotation; 	//the initial rotation
	private float 		initialPosX = 0;
	private float 		initialPosY = 0;
	private float 		initialRotX = 0;
	private float 		initialRotY = 0;

	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public bool EnableMouseRotate {
		get {
			return this.canRotate;
		}
		set {
			canRotate = value;
		}
	}

	public bool EnableKeyPanning {
		get {
			return this.canMove;
		}
		set {
			canMove = value;
		}
	}

	public bool EnableMouseZoom {
		get {
			return this.canZoom;
		}
		set {
			canZoom = value;
		}
	}
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		initialPosition = transform.position;
		initialRotation = transform.rotation;
	}

	void Update () {
		//reset camera transform
		if(Input.GetKeyUp(KeyCode.Escape))
			resetCam();

		//Camera rotation
		if(canRotate)
			handleRotation();

		//Camera movement
		if(canMove)
			handleMovement();

		//Camera zoom
		if(canZoom)
			handleZoom ();

		//constraints
		float x = Mathf.Clamp(transform.position.x, minPosX, maxPosX);
		float z = Mathf.Clamp(transform.position.z, minPosZ, maxPosZ);
		transform.position = new Vector3(x, transform.position.y, z);
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void handleRotation(){
		if(Input.GetMouseButtonDown(1)){
			initialPosX = Input.mousePosition.x;
			initialPosY = Input.mousePosition.y;
			initialRotX = transform.eulerAngles.y;
			initialRotY = transform.eulerAngles.x;
		}
		
		if(Input.GetMouseButton(1)){
			float deltaX = Input.mousePosition.x-initialPosX;
			float deltaRotX = (0.1f * (initialRotX/Screen.width));
			float rotX=deltaX+deltaRotX;
			
			float deltaY = initialPosY - Input.mousePosition.y;
			float deltaRotY = - (0.1f * (initialRotY/Screen.height));
			float rotY = deltaY + deltaRotY;
			float y = rotY + initialRotY;
			
			//rotation constraints
			if(y > maxRotAngle){
				initialRotY -= (rotY + initialRotY) - maxRotAngle;
				y = maxRotAngle;
			}
			else if(y < minRotAngle){
				initialRotY += minRotAngle - (rotY + initialRotY);
				y = minRotAngle;
			}
			
			transform.rotation = Quaternion.Euler(y, rotX + initialRotX, 0);
		}
	}

	private void handleMovement(){
		Quaternion direction=Quaternion.Euler(0, transform.eulerAngles.y, 0);

		if(Input.GetButton("Horizontal")) {
			Vector3 dir = transform.InverseTransformDirection(direction * Vector3.right);
			transform.Translate (dir * movementSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal"));
		}
		
		if(Input.GetButton("Vertical")) {
			Vector3 dir = transform.InverseTransformDirection(direction * Vector3.forward);
			transform.Translate (dir * movementSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical"));
		}
	}

	private void handleZoom(){
		if(Input.GetAxis("Mouse ScrollWheel")<0){
			if(Vector3.Distance(transform.position, refTransformZoom.position) < maxZoom){
				transform.Translate(Vector3.forward * zoomSpeed * Input.GetAxis("Mouse ScrollWheel"));
			}
		}
		else if(Input.GetAxis("Mouse ScrollWheel") > 0){
			if(Vector3.Distance(transform.position, refTransformZoom.position) > minZoom){
				transform.Translate(Vector3.forward * zoomSpeed * Input.GetAxis("Mouse ScrollWheel"));
			}
		}
	}

	private void resetCam(){
		transform.position = initialPosition;
		transform.rotation = initialRotation;
	}
}
