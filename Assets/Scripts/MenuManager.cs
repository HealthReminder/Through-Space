using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour {
    [Header("Dev Only")]
    [SerializeField]
    bool overrideConfig;
    [SerializeField]
    int newMaxLevel;

    [SerializeField]
    Transform buttonContainer;
    [SerializeField]
    float barSize;
    [Header("player Information")]
    [SerializeField]
    int maxLevel, currentLevel;
	[Header("GUI")]
    [SerializeField]
    Image overlay;
    //ScrollBar
    [SerializeField]
    Scrollbar mapScrollBar;
    [SerializeField]
    Transform[] mapParallax;
    float tCameraIY;
    [SerializeField]
    Transform lastLevel;
    float lastLevelIY;
    [Header("Buttons")]
    [SerializeField]
    Button[] buttons;
    [SerializeField]
    GameObject[] attachedGUIs;


    void Start() {

        //Dev Only 
        if (overrideConfig)
        {
            PlayerPrefs.SetInt("currentLevel", 0);
            PlayerPrefs.SetInt("maxLevel", 0);
        }
       

		//Get current max level reached
		 maxLevel = PlayerPrefs.GetInt("maxLevel");

        UpdateMenuButtons();
        
        //Play menu soundtrack
        SoundtrackManager.instance.ChangeSet("Menu");
    }
	
    //This funtion turns on the rightful menu buttons
	void UpdateMenuButtons () {
        print("Updating buttons");
		//Make the right buttons become interactable accordingly to the max level the player reached before
        for(int a = 0; a < buttons.Length; a++)
        {
            if (a <= maxLevel)
            {
                buttons[a].interactable = true;
                if (attachedGUIs[a])
                    attachedGUIs[a].SetActive(true);
            }
            else
            {
                buttons[a].interactable = false;
                if (attachedGUIs[a])
                    attachedGUIs[a].SetActive(false);
            }
               
           
        }
	}
   
	//This class will be called by the buttons to load a new scene
	public void SetGoToLevel(int levelID) {
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
     private void ResetLevels()
    {
        PlayerPrefs.SetInt("maxLevel", 0);
        PlayerPrefs.SetInt("currentLevel", 0);
        UpdateMenuButtons();
    }

     private void UnlockLevels()
    {
        PlayerPrefs.SetInt("maxLevel", 30);
        PlayerPrefs.SetInt("currentLevel", 0);
        UpdateMenuButtons();
    }
}
