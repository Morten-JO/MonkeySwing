using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCollider : MonoBehaviour {

	void Start () {
		
	}

	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if(other.tag != "Player" && other.tag != "disc" && other.GetComponent<Rigidbody>() != null){
			if (!other.isTrigger) {
				GameObject player = GameObject.FindWithTag ("Player");
				player.GetComponent<RopeSwingScript> ().notifyCollision (this.gameObject);
			}
		}
	}
}
