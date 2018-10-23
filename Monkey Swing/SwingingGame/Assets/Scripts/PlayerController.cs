using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 100f;
	private float oldMousePosition;
	public GameObject toHinge;	
	public GameObject hingeCube;
	public GameObject hingeCube2;
	private bool hasHinge = false;
	public GameObject  camera;
	public float cameraRange;
	private float oldMouseYPosition;
	private float pitch = 0.0f;


	// Use this for initialization
	void Start () {
		oldMousePosition = Input.GetAxis ("Mouse X");
		oldMouseYPosition = Input.GetAxis ("Mouse Y");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
			transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {
			transform.Translate (Vector3.back * moveSpeed * Time.deltaTime);
		} 
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.Space)) {
			this.GetComponent<Rigidbody>().AddForce (new Vector3 (0, 300, 0), ForceMode.Impulse);
		}
		if (Input.GetKey (KeyCode.E)) {
			toHinge.GetComponent<HingeJoint> ().connectedBody = hingeCube.GetComponent<Rigidbody>();
		}
		if (Input.GetKey (KeyCode.Q)) {
			toHinge.GetComponent<HingeJoint> ().connectedBody = hingeCube2.GetComponent<Rigidbody>();
		}

		float newMousePosition = Input.GetAxis ("Mouse X");
		if (newMousePosition != oldMousePosition) {
			Vector3 mov = new Vector3 (0f, newMousePosition, 0f);
			oldMousePosition = newMousePosition;
			transform.Rotate ( mov);
		}
		/*float newMouseYPosition = Input.GetAxis ("Mouse Y");
		if (newMouseYPosition != oldMouseYPosition) {
			pitch -= 2.0f * newMouseYPosition;
			camera.transform.eulerAngles = new Vector3 (pitch, 0f, 0f);
		}*/

		float newYMousePosition = Input.GetAxis ("Mouse Y");
		camera.gameObject.transform.Rotate (Vector3.right * newYMousePosition);
		if (camera.transform.localEulerAngles.x > cameraRange && camera.transform.localEulerAngles.x < 200f) {
			camera.transform.localEulerAngles = new Vector3 (cameraRange, 0, 0);
		} else if (camera.transform.localEulerAngles.x < 360f - cameraRange  && camera.transform.localEulerAngles.x > 200f) {
			camera.transform.localEulerAngles = new Vector3 (-cameraRange, 0, 0);
		}
		/*if (camera.gameObject.transform.rotation.x > cameraMaxX) {
			Quaternion old = camera.gameObject.transform.rotation;
			camera.gameObject.transform.rotation = new Quaternion (cameraMaxX, 0f, 0f, 0f);
		}
		if (camera.gameObject.transform.rotation.x < cameraMinX) {
			Quaternion old = camera.gameObject.transform.rotation;
			camera.gameObject.transform.rotation = new Quaternion (cameraMinX, 0f, 0f, 0f);
		}*/





	}
}
