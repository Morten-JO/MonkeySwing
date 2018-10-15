using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class FinishGoal : MonoBehaviour {

	public GameObject player;
	private Vector3 playerStartLocation;
	public GameObject scoreBoard;
	public GameObject videoPlayer;
	public Camera scoreBoardCamera;
	private float startTime;
	public GameObject crosshair;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		playerStartLocation = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Player") {
			crosshair.GetComponent<Image> ().enabled = false;
			print ("called?");
			scoreBoard.SetActive (true);
			scoreBoardCamera.enabled = true;
			collision.gameObject.GetComponentInChildren<Camera> ().enabled = false;
			videoPlayer.GetComponent<VideoPlayer> ().enabled = false;
			float secondsUsed = Time.time - startTime;
			int ropesUsed = player.GetComponent<RopeSwingScript> ().getRopesUsed ();
			scoreBoard.GetComponent<ScoreboardScript> ().updateScoreBoard (secondsUsed, ropesUsed);
			float distance = Vector3.Distance (playerStartLocation, player.transform.position);
		}
	}
}
