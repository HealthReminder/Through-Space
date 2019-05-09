using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour {
	public bool isOn,canLookWide,canZoom =true;
	public PlayerManager player;
	Transform playerTransform;
	public Camera followCamera,wideCamera;
	public bool isFollowEnabled = true;
	public bool isAligningWithStar = false;

    [SerializeField]	Scrollbar scrollbar;
    [SerializeField]	int orthographicSizeMin , orthographicSizeMax;
    //1 //45


    void Start () {
        scrollbar.value = 0.33f;
        ChangeCameraZoom();
		followCamera.enabled=true;
		wideCamera.enabled=false;
		playerTransform = player.transform;

	}
	void Update ()
	{
		Vector3 pos = player.orbitingStar.transform.position;
		pos.z = transform.position.z;
		transform.position = pos;
        //If the camera can follow (player is not fast in time) or if there is no orbiting planet then follow the player
        if (isFollowEnabled || !player.orbitingNow)
            followCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, followCamera.transform.position.z);
        else
        {
            //Else follow the planet the player is orbiting
            if (player.orbitingNow)
                followCamera.transform.position = new Vector3(player.orbitingNow.transform.position.x, player.orbitingNow.transform.position.y, followCamera.transform.position.z);
        }

		if(Input.GetAxis("Mouse ScrollWheel") > 0f ){
			scrollbar.value-= 0.05f;
		} else if(Input.GetAxis("Mouse ScrollWheel") < 0f ){
			scrollbar.value+= 0.05f;
		}
		//if(Input.GetMouseButton(1)){
		//	ToggleWideCamera();
		//	Time.timeScale = 6;
		//}
        
    }

    //When the player holds on the button
    public void ToggleWideCamera()
    {
		if(player.orbitingStar){
			Vector3 newPos = player.orbitingStar.position;
			newPos.z = wideCamera.transform.position.z;
			wideCamera.transform.position = newPos;
		}
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

	public void AlignToStar(Transform star) {
		if(!isAligningWithStar){
			isAligningWithStar = true;
			StartCoroutine(AlignToTransform(star));
		}
	}

	IEnumerator AlignToTransform(Transform t) {
		int iterations = 15;
		
		while(iterations > 0) {
			Vector3 middleVector3 = (2*t.up) + transform.up;
			transform.rotation = Quaternion.FromToRotation(Vector3.up, middleVector3);
			iterations--;	
			yield return new WaitForSeconds(0.02f);		
		}
		//float progress = 0;

		//while(progress <= 1) {
		//	progress += Time.deltaTime;

		///	Vector3 thisZVector = new Vector3(0,t.rotation.y,0);
		///	Vector3 tZVector = new Vector3(0,t.rotation.y,0);
		///	Vector3 newRot = Vector3.Lerp(thisZVector,tZVector, 1);
		///	transform.rotation = Quaternion.FromToRotation(Vector3.up,newRot);
///
			/// 
		//	yield return new WaitForSeconds(0.5f);
		//	yield return null;
		//}
		//yield return null;
		//transform.rotation = Quaternion.FromToRotation(Vector3.up, t.up);
		isAligningWithStar = false;
		yield break;
	}
}