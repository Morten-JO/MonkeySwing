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
	public GameObject canvas;
	public bool isGeneration = false;
	public int levelReached;

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
			canvas.SetActive (false);
			print ("called?");
			scoreBoard.SetActive (true);
			scoreBoardCamera.enabled = true;
			scoreBoardCamera.gameObject.SetActive (true);
			collision.gameObject.GetComponentInChildren<Camera> ().enabled = false;
			float secondsUsed = Time.time - startTime;
			int ropesUsed = player.GetComponent<PlayerScore> ().getRopesUsed ();
			scoreBoard.GetComponent<ScoreboardScript> ().updateScoreBoard (secondsUsed, ropesUsed);
			float distance = Vector3.Distance (playerStartLocation, player.transform.position);
			if (!isGeneration) {
				int reachedLevel = PlayerPrefs.GetInt ("reachedLevel", 1256);
				if (reachedLevel != 1256) {
					if (reachedLevel < levelReached) {
						PlayerPrefs.SetInt ("reachedLevel", levelReached);
					}
				} else {
					PlayerPrefs.SetInt ("reachedLevel", levelReached);
				}
			}
		}
	}
}
