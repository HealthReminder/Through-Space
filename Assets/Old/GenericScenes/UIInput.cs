using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInput : MonoBehaviour {
	public bool isOn,canInputNames,canInputFields,canInputAssist = true;
	public ObjectEnabler names,assist, fields;
	void Update () {
		if (isOn) {
			if (canInputNames)
			if (Input.GetKeyDown (KeyCode.Alpha1))
				names.Toggle ();
			if (canInputFields)
			if (Input.GetKeyDown (KeyCode.Alpha2))
				fields.Toggle ();
			if (canInputAssist)
			if (Input.GetKeyDown (KeyCode.Alpha3))
				assist.Toggle ();
		}
	}

	public void AddPlayerToVector () {
		assist.GetComponent<ObjectEnabler>().objects.Add(FindObjectOfType<TutorialSpaceship>().transform.GetChild(0).gameObject);
	}

}
