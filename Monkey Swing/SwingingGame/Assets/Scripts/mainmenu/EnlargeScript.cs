using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnlargeScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnPointerEnter(PointerEventData eventData){
		this.transform.localScale = new Vector3 (1.2f, 1.2f, 1f);
	}

	public void OnPointerExit(PointerEventData eventData){
		this.transform.localScale = new Vector3 (1f, 1f, 1f);
	}
}
