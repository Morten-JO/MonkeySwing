using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlScript : MonoBehaviour {

	private readonly string defaultShootRopeKey = "z";
	private readonly string defaultCancelRopeKey = "x";
	private readonly string defaultAscendRopeKey = "c";
	private readonly string defaultDescendRopeKey = "v";

	public static readonly string defaultShootRopeKeyString = "shootRope";
	public static readonly string defaultCancelRopeKeyString = "cancelRope";
	public static readonly string defaultAscendRopeKeyString = "ascendRope";
	public static readonly string defaultDescendRopeKeyString = "descendRope";

	private readonly string nonExistString = "non-exist";

	public UIControlScripts controller;

	public InputField shootRopeInput;
	public InputField cancelRopeInput;
	public InputField ascendRopeInput;
	public InputField descendRopeInput;

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

		fields = new InputField[]{shootRopeInput, cancelRopeInput, ascendRopeInput, descendRopeInput };
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
		PlayerPrefs.Save ();

		shootRopeInput.text = PlayerPrefs.GetString (defaultShootRopeKeyString);
		cancelRopeInput.text = PlayerPrefs.GetString (defaultCancelRopeKeyString);
		ascendRopeInput.text = PlayerPrefs.GetString (defaultAscendRopeKeyString);
		descendRopeInput.text = PlayerPrefs.GetString (defaultDescendRopeKeyString);
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
		PlayerPrefs.Save ();
		controller.returnMainMenu ();
	}
}
