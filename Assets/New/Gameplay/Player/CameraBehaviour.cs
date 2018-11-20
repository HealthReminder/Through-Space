using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {
	public bool isOn,canLookWide,canZoom =true;
	public PlayerBehaviour player;
	public Camera followCamera,wideCamera;
	public bool isFollowEnabled = true;
	void Start () {
		followCamera.enabled=true;
		wideCamera.enabled=false;

	}
	void Update ()
	{
		const int orthographicSizeMin =2;
		const int orthographicSizeMax =30;
		
		if (isOn) {
			//print("3");
			//if (player == null) {
				//print("4");
				if (canLookWide) {
					//print("5");
					if (Input.GetMouseButton (1)) {
						//print("1");
						if(!wideCamera.enabled){
							print("turned on wide");
							wideCamera.enabled= true;
							followCamera.enabled= false;
						}
						

					} else {
						//Follow tlayer
						if(isFollowEnabled || !player.orbitingNow)
						followCamera.transform.position = new Vector3(player.transform.position.x,player.transform.position.y,followCamera.transform.position.z);
						else{
							if(player.orbitingNow)
								followCamera.transform.position = new Vector3(player.orbitingNow.transform.position.x,player.orbitingNow.transform.position.y,followCamera.transform.position.z);
						}
						//print("2");
						if(wideCamera.enabled){
							print("turned follow");
							wideCamera.enabled= false;
							followCamera.enabled=true;
						}
					}
				} else {
				
				}
			//}

			if (canZoom) {
				if (Input.GetAxis ("Mouse ScrollWheel") < 0) { // forward
					followCamera.orthographicSize++;
				}
				if (Input.GetAxis ("Mouse ScrollWheel") > 0) { // back
					followCamera.orthographicSize--;
				}
			}

			followCamera.orthographicSize =Mathf.Clamp (followCamera.orthographicSize, orthographicSizeMin, orthographicSizeMax);
		}
	}

}