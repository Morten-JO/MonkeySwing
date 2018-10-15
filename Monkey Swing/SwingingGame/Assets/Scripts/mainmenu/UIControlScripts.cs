using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControlScripts : MonoBehaviour {

	public Text playText; 
	public Canvas mainCanvas;
	public Canvas controlCanvas;

	// Use this for initialization
	void Start () {
		mainCanvas.gameObject.SetActive (true);
		controlCanvas.gameObject.SetActive (false);
		int reachedLevel = PlayerPrefs.GetInt ("reachedLevel", 1256);
		if (reachedLevel != 1256) {
			playText.text = "Continue";
		} else {
			playText.text = "Start game";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayGame(){
		int reachedLevel = PlayerPrefs.GetInt ("reachedLevel", 1256);
		if (reachedLevel != 1256) {
			//play level 1
			SceneManager.LoadScene("LevelOneScene");
		} else {
			//continue from previous
			SceneManager.LoadScene("LevelOneScene");
			//todo
		}
	}

	public void LevelSelect(){

	}

	public void Highscore(){

	}

	public void About(){

	}

	public void Controls(){
		mainCanvas.gameObject.SetActive (false);
		controlCanvas.gameObject.SetActive (true);
	}

	public void ExitGame(){

	}

	public void returnMainMenu(){
		mainCanvas.gameObject.SetActive (true);
		controlCanvas.gameObject.SetActive (false);
	}
}
