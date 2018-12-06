using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour {

	public Transform buttonContainer;
	[Header("player Information")]
	public int maxLevel, currentLevel;
	[Header("GUI")]
	public Image overlay;
	
	void Start () {
		//For test only, set the maxLevel to 2
		PlayerPrefs.SetInt("maxLevel",3);
		//Get current max level reached
		maxLevel = PlayerPrefs.GetInt("maxLevel");
		//Make the right buttons become interactable accordingly to the max level the player reached before
		UpdateMenuButtons();
	}
	
	void UpdateMenuButtons () {
		//Takes all buttons
		Button[] buttons = buttonContainer.GetComponentsInChildren<Button>();
		//Make them interactable or not accordingly
		foreach(Button b in buttons) {
			if(int.Parse(b.name) <= maxLevel)
				b.interactable = true;
			else 
				b.interactable = false;
		}
	}

	//This class will be called by the buttons to load a new scene
	public void SetGoToLevel(int levelID) {
		currentLevel = levelID;
		PlayerPrefs.SetInt("currentLevel", currentLevel);
		StartCoroutine(Travel());
	}

	IEnumerator Travel()
	{
		overlay.gameObject.SetActive(true);
		overlay.color = new Color(0,0,0,0);
		AsyncOperation loadingScene = SceneManager.LoadSceneAsync("SpaceTravel");
		loadingScene.allowSceneActivation = false;
		while(overlay.color.a < 1){
			overlay.color+= new Color(0,0,0,0.05f);
			yield return null;
		}
		yield return new WaitForSeconds(1);
		loadingScene.allowSceneActivation = true;
		yield return loadingScene;
		
		yield return null;
	}
}
