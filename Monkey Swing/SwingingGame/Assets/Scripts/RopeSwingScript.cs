using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RopeSwingScript : MonoBehaviour {
	private int ropesUsed;
	public GameObject ropeBit;
	private float ropeBitLength;
	public GameObject targettedObject;
	private bool exist = false;
	public float maxDistanceWeb;
	public GameObject hingeObject;
	private List<GameObject> instantiatedRope = new List<GameObject> ();
	public Camera camera;
	private bool isSwinging = false;
	public GameObject image;
	private Vector3 startPosition;
	private int jointIndex;

	private KeyCode shootRopeKey;
	private KeyCode cancelRopeKey;
	private KeyCode ascendRopeKey;
	private KeyCode descendRopeKey;

	// Use this for initialization
	void Start () {
		//Key redirection start
		shootRopeKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(ControlScript.defaultShootRopeKeyString).ToUpper());
		cancelRopeKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(ControlScript.defaultCancelRopeKeyString).ToUpper());
		ascendRopeKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(ControlScript.defaultAscendRopeKeyString).ToUpper());
		descendRopeKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(ControlScript.defaultDescendRopeKeyString).ToUpper());
		//Key redirection end

		Physics.IgnoreLayerCollision (9, 10);
		startPosition = this.transform.position;
		GameObject tempObj = Instantiate (ropeBit) as GameObject;
		BoxCollider collider = tempObj.GetComponent<BoxCollider> ();
		ropeBitLength = collider.bounds.size.y;
		print (ropeBitLength);
		Destroy (tempObj);
	}

	void FixedUpdate(){
		RaycastHit hitData;
		int layerMask = 1 << 9;
		layerMask = ~layerMask;
		if (Physics.Raycast (camera.transform.position, camera.transform.TransformDirection (Vector3.forward), out hitData, maxDistanceWeb, layerMask)) {
			if (hitData.collider.gameObject.GetComponent<Rigidbody> () != null) {
				image.GetComponent<Image> ().color = new Color (0f, 1f, 0f);
			}
		} else {
			image.GetComponent<Image> ().color = new Color (1f, 0f, 0f);
		}
		RopeClimb ();
	}

	void RopeClimb(){
		if (Input.GetKey (ascendRopeKey)) {
			if (isSwinging) {
				/*this.GetComponent<FixedJoint> ().connectedBody = null;
				this.transform.position = instantiatedRope [1].gameObject.transform.position;
				this.GetComponent<FixedJoint> ().connectedBody = instantiatedRope[1].GetComponent<Rigidbody> ();
				Destroy (instantiatedRope [0]);
				instantiatedRope.RemoveAt (0);*/

				if (jointIndex < instantiatedRope.Count-1 && jointIndex >= 0) {
					jointIndex++;
					this.GetComponent<FixedJoint> ().connectedBody = null;
					this.transform.position = instantiatedRope [jointIndex].gameObject.transform.position;
					this.GetComponent<FixedJoint> ().connectedBody = instantiatedRope[jointIndex].GetComponent<Rigidbody> ();

				}
			}
		}
		if (Input.GetKey (descendRopeKey)) {
			if (isSwinging) {
				if (jointIndex > 0 && jointIndex < instantiatedRope.Count) {
					instantiatedRope [0].GetComponent<Rigidbody> ().mass = 10;
					jointIndex--;

					this.GetComponent<FixedJoint> ().connectedBody = null;
					this.transform.position = instantiatedRope [jointIndex].gameObject.transform.position;
					this.GetComponent<FixedJoint> ().connectedBody = instantiatedRope[jointIndex].GetComponent<Rigidbody> ();
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (shootRopeKey) && !isSwinging) {
			RaycastHit hitData;
			int layerMask = 1 << 9;
			layerMask = ~layerMask;
			ropesUsed++;
			if (Physics.Raycast (camera.transform.position, camera.transform.TransformDirection (Vector3.forward), out hitData, maxDistanceWeb, layerMask)) {
				shootDirectRope (hitData);
			} else {
				shootFreeRope ();
			}
		}

		if (Input.GetKeyUp (cancelRopeKey)) {
			releaseRope ();
		}
	}

	private void shootDirectRope(RaycastHit hitData){
		
		isSwinging = true;
		GameObject hitObject = hitData.collider.gameObject;
		GameObject hingeable = Instantiate (hingeObject) as GameObject;
		hingeable.transform.position = hitData.point;
		double lengthToObject = Vector3.Distance (this.transform.position, hingeable.transform.position);
		GameObject lastLatched = null;
		int count = 0;
		GameObject firstLatched = null;
		for (double i = 0; i < lengthToObject - ropeBitLength; i += ropeBitLength) {
			GameObject ropeObj = Instantiate (ropeBit) as GameObject;
			ropeObj.transform.position = this.transform.position;
			ropeObj.transform.LookAt (hingeable.transform);
			ropeObj.transform.rotation *= Quaternion.Euler (90, 0, 0);
			ropeObj.transform.Translate (Vector3.up * ropeBitLength * count);
			if (lastLatched != null) {
				lastLatched.GetComponent<FixedJoint> ().connectedBody = ropeObj.GetComponent<Rigidbody> ();
			}
			lastLatched = ropeObj;
			if (count == 0) {
				firstLatched = lastLatched;
			}
			count++;
			instantiatedRope.Add (ropeObj);

			exist = true;
		}
		instantiatedRope.Add (hingeable);
		jointIndex = 0;
		lastLatched.GetComponent<FixedJoint> ().connectedBody = hingeable.GetComponent<Rigidbody> ();
		this.gameObject.AddComponent<FixedJoint> ();
		this.GetComponent<FixedJoint> ().connectedBody = firstLatched.GetComponent<Rigidbody> ();
		this.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
	}

	private void shootFreeRope(){
		Vector3 endPosition = camera.transform.position + (camera.transform.TransformDirection (Vector3.forward) * maxDistanceWeb);
		double lengthToObject = Vector3.Distance (this.transform.position, endPosition);
		isSwinging = true;
		GameObject lastLatched = null;
		int count = 0;
		GameObject firstLatched = null;
		for (double i = 0; i < maxDistanceWeb - ropeBitLength; i += ropeBitLength) {
			GameObject ropeObj = Instantiate (ropeBit) as GameObject;
			Physics.IgnoreCollision (ropeObj.GetComponent<Collider> (), this.GetComponent<Collider> ());
			ropeObj.transform.position = this.transform.position;
			ropeObj.transform.LookAt (endPosition);
			ropeObj.transform.rotation *= Quaternion.Euler (90, 0, 0);
			ropeObj.transform.Translate (Vector3.up * ropeBitLength * count);
			if (lastLatched != null) {
				lastLatched.GetComponent<FixedJoint> ().connectedBody = ropeObj.GetComponent<Rigidbody> ();
			}
			lastLatched = ropeObj;
			if (count == 0) {
				firstLatched = lastLatched;
			}
			count++;
			instantiatedRope.Add (ropeObj);

			exist = true;
		}
		Destroy (lastLatched.GetComponent<FixedJoint> ());
		this.gameObject.AddComponent<FixedJoint> ();
		this.GetComponent<FixedJoint> ().connectedBody = firstLatched.GetComponent<Rigidbody> ();
		this.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
	}

	private void releaseRope(){
		if (isSwinging) {
			if (this.GetComponent<FixedJoint> ().connectedBody != null) {
				this.GetComponent<FixedJoint> ().connectedBody = null;
			}
			Destroy (this.GetComponent<FixedJoint> ());
			for (int i = 0; i < instantiatedRope.Count; i++) {
				Destroy (instantiatedRope [i]);
			}
			instantiatedRope.Clear ();
			this.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			this.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			isSwinging = false;
		}
	}

	public void notifyCollision(GameObject ropeInstance){
		for (int i = 0; i < instantiatedRope.Count; i++) {
			if (instantiatedRope [i].gameObject == ropeInstance) {
				if (i + 5 > instantiatedRope.Count || i - 5 < 0) {
					return;
				}
				GameObject hingeable = Instantiate (hingeObject) as GameObject;
				hingeable.transform.position = instantiatedRope [i].transform.position;
				GameObject oldShitFunWithBags = instantiatedRope [i];
				/*GameObject oldRope = instantiatedRope[0];
				for (int j = 0; j < instantiatedRope.Count; j++) {
					if (j + 1 < instantiatedRope.Count) {
						GameObject temp = oldRope;
						oldRope = instantiatedRope [j + 1];
						instantiatedRope [j + 1] = temp;
					}
				}
				instantiatedRope.Add (oldRope);*/
				instantiatedRope [i] = hingeable;
				if (i > 1) {
					instantiatedRope[i-1].GetComponent<FixedJoint> ().connectedBody = hingeable.GetComponent<Rigidbody> ();
				}
				Destroy (oldShitFunWithBags);
				jointIndex = 0;
				int removeAmount = instantiatedRope.Count - (i + 1);
				for (int j = 0; j < removeAmount; j++) {
					Destroy (instantiatedRope [i+1]);
					instantiatedRope.RemoveAt (i+1);
				}
				return;
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "terrain") {
			this.transform.position = startPosition;
			releaseRope ();
			this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}
	}

	public int getRopesUsed(){
		return ropesUsed;
	}
		
}

