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
	public Canvas aboutPage;
	//levelSelect
	public Image crossSnowLevel;
	public Slider randomMapGenerationSlider;
	public static int randomMapGenerationDifficulty = 1;

	// Use this for initialization
	void Start () {
		aboutPage.gameObject.GetComponent<Canvas> ().enabled = false;
		mainCanvas.gameObject.GetComponent<Canvas> ().enabled = true;
		controlCanvas.gameObject.GetComponent<Canvas> ().enabled = false;
		levelSelectCanvas.gameObject.GetComponent<Canvas> ().enabled = false;
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
		bool loadedScene = false;
		string text = "Level";
		if (reachedLevel < 2 || reachedLevel == 1256) {
			text += "One";
			loadedScene = true;
		} else if (reachedLevel == 2) {
			text += "Two";
			loadedScene = true;
		} else {
			print ("You have completed the game, there is no more new maps you can play.");
			//TODO put information into UI
			loadedScene = false;
		}
		 
		text += "Scene";
		if (loadedScene) {
			SceneManager.LoadScene (text);
		}
	}

	public void LevelSelect(){
		aboutPage.gameObject.GetComponent<Canvas> ().enabled = false;
		mainCanvas.gameObject.GetComponent<Canvas> ().enabled = false;
		controlCanvas.gameObject.GetComponent<Canvas> ().enabled = false;
		levelSelectCanvas.gameObject.GetComponent<Canvas> ().enabled = true;
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
		levelSelectCanvas.gameObject.GetComponent<Canvas> ().enabled = false;
		aboutPage.gameObject.GetComponent<Canvas> ().enabled = true;
		mainCanvas.gameObject.GetComponent<Canvas> ().enabled = false;
		controlCanvas.gameObject.GetComponent<Canvas> ().enabled = false;
	}

	public void Controls(){
		levelSelectCanvas.gameObject.GetComponent<Canvas> ().enabled = false;
		aboutPage.gameObject.GetComponent<Canvas> ().enabled = false;
		mainCanvas.gameObject.GetComponent<Canvas> ().enabled = false;
		controlCanvas.gameObject.GetComponent<Canvas> ().enabled = true;
	}

	public void ExitGame(){
		Application.Quit();
	}

	public void returnMainMenu(){
		levelSelectCanvas.gameObject.GetComponent<Canvas> ().enabled = false;
		aboutPage.gameObject.GetComponent<Canvas> ().enabled = false;
		mainCanvas.gameObject.GetComponent<Canvas> ().enabled = true;
		controlCanvas.gameObject.GetComponent<Canvas> ().enabled = false;
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

	public void GoRandomMap(){
		randomMapGenerationDifficulty = (int)randomMapGenerationSlider.value;
		SceneManager.LoadScene ("GenerationScene");
	}
}
