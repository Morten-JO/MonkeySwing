using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrillinScript : MonoBehaviour {

	public GameObject discPrefab;
	public GameObject player;
	public float timeBetweenAttacks;
	public float particleChargeTime;
	public float discSpeed = 10f;
	private float count;
	private GameObject currentDisc;
	// Use this for initialization
	void Start () {
		count = 0f;
		currentDisc = Instantiate (discPrefab) as GameObject;
		currentDisc.GetComponentInChildren<ParticleSystem> ().Stop ();
		currentDisc.GetComponentInChildren<ParticleSystem> ().time = 0;
		StartCoroutine (chargeDisc ());
	}
	
	// Update is called once per frame
	void Update () {
		if (count > timeBetweenAttacks) {
			GameObject[] ropes = GameObject.FindGameObjectsWithTag ("rope");
			if (ropes.Length > 0) {
				int lastConnected = 0;
				for (int i = 0; i < ropes.Length; i++) {
					lastConnected = i;
					if (ropes [i].GetComponent<FixedJoint> () != null) {
						if (ropes [i].GetComponent<FixedJoint> ().connectedBody == null) {
							break;
						}
					}
				}
				int properIndex;
				if (lastConnected - 5 > 0 && ropes.Length > lastConnected - 5) {
					properIndex = lastConnected - 5;
				} else {
					if (lastConnected - 1 < ropes.Length) {
						properIndex = lastConnected - 1;
					} else {
						properIndex = ropes.Length - 1;
					}

				}
				shootAtRope (ropes[properIndex], currentDisc);
				count = 0f;
				currentDisc = Instantiate (discPrefab) as GameObject;
				currentDisc.GetComponentInChildren<ParticleSystem> ().Stop ();
				currentDisc.GetComponentInChildren<ParticleSystem> ().time = 0;
				StartCoroutine (chargeDisc ());

			}
		} else {
			count += Time.deltaTime;
		}
	}

	private void shootAtRope(GameObject obj, GameObject currentDisc){
		currentDisc.transform.LookAt (obj.transform);
		StartCoroutine (sendDisc (10f, currentDisc));
	}

	private IEnumerator chargeDisc(){
		Vector3 origScale = currentDisc.transform.localScale;
		Vector3 destScale = new Vector3 (10.0f, 0.01f, 10.0f);

		float currentTime = 0.0f;

		do {
			currentDisc.transform.localScale = Vector3.Lerp(origScale, destScale, currentTime / (timeBetweenAttacks - particleChargeTime));
			currentTime += Time.deltaTime;
			yield return null;
		} while(currentTime <= timeBetweenAttacks - particleChargeTime);
		currentDisc.GetComponentInChildren<ParticleSystem> ().Play ();
	}

	private IEnumerator sendDisc(float sendTime, GameObject currentDisc){

		float currentTime = 0.0f;

		while (currentTime < sendTime) {
			currentDisc.transform.position += currentDisc.transform.forward * Time.deltaTime * discSpeed;
			currentTime += Time.deltaTime;
			yield return null;
		}
		Destroy (currentDisc);


	}
}
