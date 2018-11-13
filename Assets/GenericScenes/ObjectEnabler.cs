using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectEnabler : MonoBehaviour {

	public bool disableOnStart = false;
	public bool isOn = false;
	public List<GameObject> objects;
	void Start () {
		if (disableOnStart)
			Disable ();
	}

	// Update is called once per frame


	public void Disable(){
		isOn = false;
		foreach (GameObject gO in objects) {
			if (gO.GetComponent<LineRenderer> () != null)
				gO.GetComponent<LineRenderer> ().enabled = false;
			if (gO.GetComponent<SpriteRenderer> () != null)
				gO.GetComponent<SpriteRenderer> ().enabled = false;
			if (gO.GetComponent<Text> () != null)
				gO.GetComponent<Text> ().enabled = false;
		}
	}

	public void Enable(){
		isOn = true;
		foreach (GameObject gO in objects) {
			if (gO.GetComponent<LineRenderer> () != null)
				gO.GetComponent<LineRenderer> ().enabled = true;
			if (gO.GetComponent<SpriteRenderer> () != null)
				gO.GetComponent<SpriteRenderer> ().enabled = true;
			if (gO.GetComponent<Text> () != null)
				gO.GetComponent<Text> ().enabled = true;
		}
	}

	public void Toggle(){
		if (isOn == false) {
			Enable ();
		} else {
			Disable ();
		}
	}

}
