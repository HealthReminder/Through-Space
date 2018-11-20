using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour {

	public Scrollbar timeBar;
	public CameraBehaviour camBehaviour;
	
	public void ChangeTime(int newSpeed) {
		//Time.timeScale = Mathf.Pow(timeBar.value,3);`
		//If the parameter is 0 it means you should change time accordingly to the scrollbar
		if(newSpeed == 0) {
		Time.timeScale = Mathf.Pow(((timeBar.value)*2)+1,3);
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
	}

	public void PauseTime() {
		//Time.timeScale = Mathf.Pow(timeBar.value,3);
		Time.timeScale = 0;
		//
	}

	public void RestartScene() {
		 SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
