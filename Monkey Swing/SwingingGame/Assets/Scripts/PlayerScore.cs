using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

	private int ropesUsed;
	private int resets;
	private int bananasCollected;


	public Text resetText;
	public Text ropeText;
	public Text bananaText;

	// Use this for initialization
	void Start () {
		restart ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void restart(){
		ropesUsed = 0;
		resets = 0;
		bananasCollected = 0;
		updateRopesText ();
		updateResetText ();
		updateBananasText ();
	}

	public void addRopeUsed(){
		ropesUsed++;
		updateRopesText ();
	}

	public void addResetUsed(){
		resets++;
		updateResetText ();
	}

	public void addBananaCollected(){
		bananasCollected++;
		updateBananasText ();
	}

	public int getRopesUsed(){
		return ropesUsed;
	}

	public int getResetsUsed(){
		return resets;
	}

	public int getBananasCollected(){
		return bananasCollected;
	}

	public void updateRopesText(){
		ropeText.text = "Ropes used: " + ropesUsed;
	}

	public void updateResetText(){
		resetText.text = "Reset used: " + resets;
	}

	public void updateBananasText(){
		bananaText.text = "Bananas collected: " + bananasCollected;
	}
}
