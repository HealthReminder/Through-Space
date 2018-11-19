using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour {

	public Scrollbar timeBar;
	public CameraBehaviour camBehaviour;
	public void ChangeTime() {
		//Time.timeScale = Mathf.Pow(timeBar.value,3);
		Time.timeScale = Mathf.Pow(((timeBar.value)*2)+1,3);
		if(Time.timeScale != 1)
			camBehaviour.isFollowEnabled = false;
		else 
			camBehaviour.isFollowEnabled = true;
		print(Time.timeScale);
		//
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
