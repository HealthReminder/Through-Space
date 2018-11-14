using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	TutorialSpaceship player;
	public bool isOn,canFocus,canZoom = true;
	//Vector3 m
	void Start () {
		FindPlayer ();
	}
	public void FindPlayer() {
		player = FindObjectOfType<TutorialSpaceship> ();
	}
	void Update ()
	{
		const int orthographicSizeMin = 2;
		const int orthographicSizeMax = 15;
		if (isOn) {
			if (player != null) {
				if (canFocus) {

					if (Input.GetMouseButton (1)) {
						StartCoroutine (MoveToCursor (Input.mousePosition));

					} else {
						Camera.main.transform.position = player.transform.position + new Vector3 (0, 0, -20);
					}
				} else {
				
				}
			}

			if (canZoom) {


				if (Input.GetAxis ("Mouse ScrollWheel") < 0) { // forward
					Camera.main.orthographicSize++;
				}
				if (Input.GetAxis ("Mouse ScrollWheel") > 0) { // back
					Camera.main.orthographicSize--;
				}
			}

			Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax);
		}
	}

	IEnumerator MoveToCursor(Vector3 mousePos) {
		
		while (Input.GetMouseButton(1)) {
			float clampedX, clampedY;
			clampedX = Mathf.Clamp (Camera.main.ScreenToWorldPoint (mousePos).x/10, -10, 10);
			clampedY = Mathf.Clamp (Camera.main.ScreenToWorldPoint (mousePos).y/10, -10, 10);
			Camera.main.transform.position = Vector3.Lerp (new Vector3(clampedX,clampedY,-20f), Camera.main.transform.position, 0.001f);
			yield return null;
		}
		yield break;
	}
}