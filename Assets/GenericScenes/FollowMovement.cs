using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowMovement : MonoBehaviour {

	public GameObject objectToFollow;
	Vector3 initialDiff;
	Text txt;
	void Start () {
//		if (transform.GetComponent<Text> () != null)
//			txt = transform.GetComponent<Text> ();
//		if (txt != null)
//			txt.text = objectToFollow.GetComponent<Planet> ().name;
		initialDiff = transform.position;
		initialDiff = transform.position - objectToFollow.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = objectToFollow.transform.position + initialDiff;
	}
}
