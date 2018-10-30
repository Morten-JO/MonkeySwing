using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControlScripts : MonoBehaviour {

	public Text playText; 
	public Canvas mainCanvas;
	public Canvas controlCanvas;
	public Canvas levelSelectCanvas;

	//levelSelect
	public Image crossSnowLevel;

	// Use this for initialization
	void Start () {
		mainCanvas.gameObject.SetActive (true);
		controlCanvas.gameObject.SetActive (false);
		levelSelectCanvas.gameObject.SetActive (false);
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
		string text = "Level";
		if (reachedLevel < 2 || reachedLevel == 1256) {
			text += "One";
		} else if (reachedLevel == 2) {
			text += "Two";
		} else {
			print ("bugged af level select");
		}
		 
		text += "Scene";
		SceneManager.LoadScene (text);
	}

	public void LevelSelect(){
		mainCanvas.gameObject.SetActive (false);
		controlCanvas.gameObject.SetActive (false);
		levelSelectCanvas.gameObject.SetActive (true);
		int reachedLevel = PlayerPrefs.GetInt ("reachedLevel", 1256);
		if (reachedLevel >= 2 && reachedLevel != 1256) {
			crossSnowLevel.gameObject.SetActive (false);
		} else {
			crossSnowLevel.gameObject.SetActive (true);
		}

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
		Application.Quit();
	}

	public void returnMainMenu(){
		mainCanvas.gameObject.SetActive (true);
		controlCanvas.gameObject.SetActive (false);
	}

	public void GoLevelOne(){
		SceneManager.LoadScene("LevelOneScene");
	}

	public void GoLevelTwo(){
		int reachedLevel = PlayerPrefs.GetInt ("reachedLevel", 1256);
		if (reachedLevel >= 2) {
			SceneManager.LoadScene ("LevelTwoScene");
		}
	}
}
