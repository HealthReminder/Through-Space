﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour {
    public StarController[] levelInfos;
    [SerializeField]    Transform buttonContainer;
    [SerializeField]    float barSize;
    [Header("player Information")]
    [SerializeField]    int maxLevel, currentLevel;
	[Header("GUI")]
    [SerializeField]    Image overlay;
    //ScrollBar
    [SerializeField]    Scrollbar mapScrollBar;
    [SerializeField]  Transform[] mapParallax;
    float tCameraIY;
    [SerializeField]   Transform lastLevel;
    float lastLevelIY;
    [Header("Buttons")]
    [SerializeField]    Button[] buttons;

    public static MenuManager instance;
    private void Awake() {
        instance = this;
    }
    void Start() {
        Time.timeScale = 1;
        //Get current max level reached
        maxLevel = PlayerPrefs.GetInt("maxLevel");

        StartCoroutine(UpdateMenuButtons());
        
        
    }
	
    //This funtion turns on the rightful menu buttons
	IEnumerator UpdateMenuButtons () {
        print("Updating buttons");
        yield return new WaitForSeconds(Time.deltaTime*10);
		//Make the right buttons become interactable accordingly to the max level the player reached before
        Debug.Log("Max level is " + maxLevel);
        for(int a = 0; a < buttons.Length; a++)
        {
            LevelButtonBehaviour b = buttons[a].GetComponent<LevelButtonBehaviour>();
            if (a <= maxLevel)
            {
                b.levelIndex = a;
                b.UpdateView(levelInfos[a]);
            }
            else
                b.DisableView();
            
               
           yield return new WaitForSeconds(Time.deltaTime);
        }

        yield break;
	}
   
	//This class will be called by the buttons to load a new scene
	public void SetGoToLevel(int levelID) {
        //AudioManager.instance.Play("Menu_Button");
		currentLevel = levelID;
		PlayerPrefs.SetInt("currentLevel", currentLevel);
        SoundtrackManager.instance.ChangeSet("Calm");
		StartCoroutine(LoadScene("SpaceTravel"));
	}

    //This function is responsible for loading the 
    //Game scene when the player chooses a level to play
	IEnumerator LoadScene(string sceneName)
	{
		overlay.gameObject.SetActive(true);
		overlay.color = new Color(0,0,0,0);
		while(overlay.color.a < 1){
			overlay.color+= new Color(0,0,0,0.1f);
			yield return new WaitForSeconds(0.01f);
		}
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(sceneName);
        loadingScene.allowSceneActivation = true;
        while(!loadingScene.isDone)
            yield return null;
		yield return loadingScene;
		
		yield return null;
	}
    
    //DEBUGGING
    private void Update() {
        if(Input.touchCount == 6 || Input.GetKeyDown(KeyCode.P)){
            ResetLevels();
            StartCoroutine(UpdateMenuButtons());
        }
        if(Input.touchCount == 7 ||Input.GetKeyDown(KeyCode.O)){
            UnlockLevels();
            StartCoroutine(UpdateMenuButtons());
        }
    }
     private void ResetLevels()
    {
        PlayerPrefs.SetInt("maxLevel", 0);
        PlayerPrefs.SetInt("currentLevel", 0);
        maxLevel = 0;
        UpdateMenuButtons();
    }

     private void UnlockLevels()
    {
        PlayerPrefs.SetInt("maxLevel", 30);
        PlayerPrefs.SetInt("currentLevel", 0);
        maxLevel = 30;
        UpdateMenuButtons();
    }
}
