using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaScript : MonoBehaviour {

	public float speed = 1;

	// Use this for initialization
	void Start () {
		
		float rot = Random.Range (0f, 360f);
		this.transform.Rotate (0f, rot, 0f);
	
	}
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up * Time.deltaTime * speed);
	}
}
