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
    Transform tCamera;
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
       

        //Store the initial Y position of the map
       
        //Screen.SetResolution(1000, 1600, false);
		//Get current max level reached

		 maxLevel = PlayerPrefs.GetInt("maxLevel");
		
        tCameraIY = tCamera.position.y;
        if(maxLevel > 4)
        {
            mapScrollBar.interactable = true;
            mapScrollBar.gameObject.SetActive(true);
            
           //lastLevel = GameObject.Find(maxLevel.ToString()).transform;
            lastLevelIY = lastLevel.position.y;
        } else
        {
            mapScrollBar.gameObject.SetActive(false);
            mapScrollBar.interactable = false;
        }

        //Make the right buttons become interactable accordingly to the max level the player reached before
        UpdateMenuButtons();
        MoveCamera();
    }
	
	void UpdateMenuButtons () {
        print("Updating buttons");
		//Make them interactable or not accordingly
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
    public void ResetGame()
    {
        PlayerPrefs.SetInt("maxLevel", 0);
        PlayerPrefs.SetInt("currentLevel", 0);
    }
	//This class will be called by the buttons to load a new scene
	public void SetGoToLevel(int levelID) {
		currentLevel = levelID;
		PlayerPrefs.SetInt("currentLevel", currentLevel);
		StartCoroutine(Travel());
	}

    public void MoveCamera()
    {
        //print("Moving camera, " + Mathf.Lerp(0, lastLevelIY, mapScrollBar.value)+ "  " + lastLevelIY + "  pos "+ tCameraIY);
        tCamera.position = new Vector3(tCamera.position.x, tCameraIY - Mathf.Lerp(0, lastLevelIY - 500, mapScrollBar.value) );
       // for(int a = 0; a < mapParallax.Length; a++)
           // mapParallax[a].position = new Vector3(sBInitialY.x, sBInitialY.y*2/(a+1) - mapScrollBar.value*100*a*a , 0);

    }

	IEnumerator Travel()
	{
		overlay.gameObject.SetActive(true);
		overlay.color = new Color(0,0,0,0);
		while(overlay.color.a < 1){
			overlay.color+= new Color(0,0,0,0.05f);
			yield return null;
		}
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync("SpaceTravel");
        loadingScene.allowSceneActivation = false;
        yield return new WaitForSeconds(1);
		loadingScene.allowSceneActivation = true;
		yield return loadingScene;
		
		yield return null;
	}
}
