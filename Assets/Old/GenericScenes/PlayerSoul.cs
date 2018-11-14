using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoul : MonoBehaviour {

	TutorialSpaceship player;
	public void FindPlayer () {
		player = FindObjectOfType<TutorialSpaceship> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(player !=null)
			transform.position = player.transform.position;
	}

	void OnTriggerStay2D (Collider2D coll) {
		if (coll.tag == "surface")
			player.StartCoroutine ("Die");
	}
}
