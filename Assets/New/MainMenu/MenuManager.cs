using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour {

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
    Transform mapTransform;
    [SerializeField]
    Transform[] mapParallax;
    float mapY;
	
	void Start () {
       
        //Dev Only 
        PlayerPrefs.SetInt("currentLevel", 1);
        PlayerPrefs.SetInt("maxLevel", 10);

        //Store the initial Y position of the map
       
        //Screen.SetResolution(1000, 1600, false);
		//Get current max level reached
		maxLevel = PlayerPrefs.GetInt("maxLevel");
		//Make the right buttons become interactable accordingly to the max level the player reached before
		UpdateMenuButtons();
        mapY = mapTransform.position.y;
        MoveMap();
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

    public void MoveMap()
    {
        mapTransform.position = new Vector3(mapTransform.position.x, mapY - mapScrollBar.value * barSize, 0);
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
