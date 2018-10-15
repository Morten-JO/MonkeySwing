using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardScript : MonoBehaviour {
	private string timeSpentText = "Time spent: ";
	private string ropesUsedText = "Ropes used: ";
	public Text timeSpentObject;
	public Text ropesUsedObject;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void updateScoreBoard(float seconds, int ropes){
		this.enabled = true;
		string toScoreboardTimeSpent = timeSpentText + seconds + " seconds!";
		string toScoreboardRopesUsed = ropesUsedText + ropes + "!";
		StartCoroutine(updateTextOverTime(toScoreboardTimeSpent, timeSpentObject));
		StartCoroutine(updateTextOverTime(toScoreboardRopesUsed, ropesUsedObject));
	}

	private IEnumerator updateTextOverTime(string text, Text textObject){
		string putOnBoard = "";
		for (int i = 0; i < text.Length; i++) {
			putOnBoard += text.ToCharArray() [i];
			textObject.text = putOnBoard;
			yield return new WaitForSeconds (0.1f);
		}
	}
}
