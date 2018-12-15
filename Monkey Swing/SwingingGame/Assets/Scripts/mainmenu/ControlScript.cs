using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlScript : MonoBehaviour {

	public static readonly string defaultShootRopeKey = "z";
	public static readonly string defaultCancelRopeKey = "x";
	public static readonly string defaultAscendRopeKey = "c";
	public static readonly string defaultDescendRopeKey = "v";
	public static readonly string defaultBoostKey = "b";

	public static readonly string defaultShootRopeKeyString = "shootRope";
	public static readonly string defaultCancelRopeKeyString = "cancelRope";
	public static readonly string defaultAscendRopeKeyString = "ascendRope";
	public static readonly string defaultDescendRopeKeyString = "descendRope";
	public static readonly string defaultBoostKeyString = "boost";

	private readonly string nonExistString = "non-exist";

	public UIControlScripts controller;

	public InputField shootRopeInput;
	public InputField cancelRopeInput;
	public InputField ascendRopeInput;
	public InputField descendRopeInput;
	public InputField boostKeyInput;

	private InputField[] fields;

	//redColorBlock
	private ColorBlock redBlock;
	//standardBlock
	private ColorBlock normalBlock;

	// Use this for initialization
	void Start () {
		redBlock = shootRopeInput.colors;
		Color red = new Color (1f, 0f, 0f, 1f);
		redBlock.normalColor = red;
		redBlock.highlightedColor = red;
		redBlock.disabledColor = red;
		redBlock.pressedColor = red;

		normalBlock = shootRopeInput.colors;
		Color normal = new Color (1f, 1f, 1f, 1f);
		normalBlock.normalColor = normal;
		normalBlock.highlightedColor = normal;
		normalBlock.disabledColor = normal;
		normalBlock.pressedColor = normal;

		fields = new InputField[]{shootRopeInput, cancelRopeInput, ascendRopeInput, descendRopeInput, boostKeyInput };
		string m_shootRope = PlayerPrefs.GetString (defaultShootRopeKeyString, nonExistString);
		if (m_shootRope == nonExistString) {
			PlayerPrefs.SetString (defaultShootRopeKeyString, defaultShootRopeKey);
		}
		string m_cancelRope = PlayerPrefs.GetString (defaultCancelRopeKeyString, nonExistString);
		if (m_cancelRope == nonExistString) {
			PlayerPrefs.SetString (defaultCancelRopeKeyString, defaultCancelRopeKey);
		}
		string m_ascendRope = PlayerPrefs.GetString (defaultAscendRopeKeyString, nonExistString);
		if (m_ascendRope == nonExistString) {
			PlayerPrefs.SetString (defaultAscendRopeKeyString, defaultAscendRopeKey);
		}
		string m_descendRope = PlayerPrefs.GetString (defaultDescendRopeKeyString, nonExistString);
		if (m_descendRope == nonExistString) {
			PlayerPrefs.SetString (defaultDescendRopeKeyString, defaultDescendRopeKey);
		}
		string m_boost = PlayerPrefs.GetString (defaultBoostKeyString, nonExistString);
		if (m_boost == nonExistString) {
			PlayerPrefs.SetString (defaultBoostKeyString, defaultBoostKey);
		}
		PlayerPrefs.Save ();

		shootRopeInput.text = PlayerPrefs.GetString (defaultShootRopeKeyString);
		cancelRopeInput.text = PlayerPrefs.GetString (defaultCancelRopeKeyString);
		ascendRopeInput.text = PlayerPrefs.GetString (defaultAscendRopeKeyString);
		descendRopeInput.text = PlayerPrefs.GetString (defaultDescendRopeKeyString);
		boostKeyInput.text = PlayerPrefs.GetString (defaultBoostKeyString);
		InvokeRepeating ("checkFields", 0.5f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void checkFields(){
		if (validateInputField (shootRopeInput)) {
			shootRopeInput.colors = normalBlock;
		} else {
			shootRopeInput.colors = redBlock;
		}
		if (validateInputField (cancelRopeInput)) {
			cancelRopeInput.colors = normalBlock;
		} else {
			cancelRopeInput.colors = redBlock;
		}
		if (validateInputField (ascendRopeInput)) {
			ascendRopeInput.colors = normalBlock;
		} else {
			ascendRopeInput.colors = redBlock;
		}
		if (validateInputField (descendRopeInput)) {
			descendRopeInput.colors = normalBlock;
		} else {
			descendRopeInput.colors = redBlock;
		}
		if (validateInputField (boostKeyInput)) {
			boostKeyInput.colors = normalBlock;
		} else {
			boostKeyInput.colors = redBlock;
		}
	}

	public bool validateInputField(InputField field){
		if (field.text.Length == 1) {
			bool sameKeyBind = false;
			for (int i = 0; i < fields.Length; i++) {
				if (field != fields [i]) {
					if (field.text == fields [i].text) {
						sameKeyBind = true;
					}
				}
			}
			if (sameKeyBind) {
				return false;
			} else {
				return true;
			}
		}
		return false;
	}

	public void returnMainMenu(){
		if (validateInputField (shootRopeInput)) {
			PlayerPrefs.SetString (defaultShootRopeKeyString, shootRopeInput.text);
		}
		if (validateInputField (cancelRopeInput)) {
			PlayerPrefs.SetString (defaultCancelRopeKeyString, cancelRopeInput.text);
		}
		if (validateInputField (ascendRopeInput)) {
			PlayerPrefs.SetString (defaultAscendRopeKeyString, ascendRopeInput.text);
		}
		if (validateInputField (descendRopeInput)) {
			PlayerPrefs.SetString (defaultDescendRopeKeyString, descendRopeInput.text);
		}
		if (validateInputField (boostKeyInput)) {
			PlayerPrefs.SetString (defaultBoostKeyString, boostKeyInput.text);
		}
		PlayerPrefs.Save ();
		controller.returnMainMenu ();
	}

	public void returnToMainMenuFromAbout(){
		controller.returnMainMenu ();
	}
		
}
