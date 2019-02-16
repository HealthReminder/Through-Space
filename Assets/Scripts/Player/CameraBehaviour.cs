using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraBehaviour : MonoBehaviour {
	public bool isOn,canLookWide,canZoom =true;
	public PlayerBehaviour player;
	public Camera followCamera,wideCamera;
	public bool isFollowEnabled = true;

    [SerializeField]
    Scrollbar scrollbar;
    [SerializeField]
    int orthographicSizeMin , orthographicSizeMax;
    //1 //45


    void Start () {
        scrollbar.value = 0.33f;
        ChangeCameraZoom();
		followCamera.enabled=true;
		wideCamera.enabled=false;

	}
	void Update ()
	{
        //If the camera can follow (player is not fast in time) or if there is no orbiting planet then follow the player
        if (isFollowEnabled || !player.orbitingNow)
            followCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, followCamera.transform.position.z);
        else
        {
            //Else follow the planet the player is orbiting
            if (player.orbitingNow)
                followCamera.transform.position = new Vector3(player.orbitingNow.transform.position.x, player.orbitingNow.transform.position.y, followCamera.transform.position.z);
        }
        /*
		
		if (isOn) {
				if (canLookWide) {
					if (Input.GetMouseButton (1)) {
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

			//if (canZoom) {
				//if (Input.GetAxis ("Mouse ScrollWheel") < 0) { // forward
					followCamera.orthographicSize++;
				//}
				//if (Input.GetAxis ("Mouse ScrollWheel") > 0) { // back
					followCamera.orthographicSize--;
				//}
			//}
            */
        //}
    }

    //When the player holds on the button
    public void ToggleWideCamera()
    {
        wideCamera.enabled = true;
        followCamera.enabled = false;
}

    //When the player releases button
    public void ToggleFollowCamera()
    {
         wideCamera.enabled = false;
         followCamera.enabled = true;
}

    //When the player changes the scrollbar
    public void ChangeCameraZoom()
    {
        followCamera.orthographicSize = Mathf.Lerp(orthographicSizeMin, orthographicSizeMax, scrollbar.value);
    }
}