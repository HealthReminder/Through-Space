using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour {
	public bool isOn;
	public CameraBehaviour camBehaviour;
	
	public void ChangeTime(int newSpeed) {
		if(isOn){
			//Time.timeScale = Mathf.Pow(timeBar.value,3);`
			//If the parameter is 0 it means you should change time accordingly to the scrollbar
			if(newSpeed == 1) {
				Time.timeScale = 1;
			//print("Slowed time up to: " + Time.timeScale);
			} else {
				Time.timeScale = newSpeed;
				//print("Sped time up to: " + Time.timeScale);
			}

			//Fix camera on planet to not make the player sick!
				if(Time.timeScale != 1)
					camBehaviour.isFollowEnabled = false;
				else 
					camBehaviour.isFollowEnabled = true;
		}
	}

	public void PauseTime() {
		//Time.timeScale = Mathf.Pow(timeBar.value,3);
		Time.timeScale = 0;
		//
	}

	public void RestartScene() {
		 SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

/*public void NewTime(int newSpeed) {
		//Time.timeScale = Mathf.Pow(timeBar.value,3);`
		//If the parameter is 0 it means you should change time accordingly to the scrollbar
		if(newSpeed == 0) {
		Time.timeScale = Mathf.Pow(((timeBar.value)*2)+1,3);
            print(timeBar.value + " accelerating to " + Time.timeScale);
		if(Time.timeScale != 1)
			camBehaviour.isFollowEnabled = false;
		else 
			camBehaviour.isFollowEnabled = true;
		} else {
			//Else change the speed on the scrollbar to the parameter
			timeBar.value = ((float)newSpeed-1)/3;
			print("Changed time: " +timeBar.value);
			ChangeTime(0);
			timeBar.interactable = false;
		}
	} */
	

	
}
