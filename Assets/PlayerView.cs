using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{   
    public bool isProximityOn = false;
    public Image proximityGUI;

      
    //Make it a singleton
    public static PlayerView instance;
    void Awake(){
		//Singleton pattern
		if  (instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
		}	
		else if (instance != this)
			Destroy(gameObject);
		
	}

    public void ToggleProximity (float percentage){
       // proximityGUI.color = new Color(percentage,1-percentage,0,1);
        float hue = Mathf.Lerp(-0.065f,0.45f,1-percentage);
        Debug.Log(percentage);
        proximityGUI.color = Color.HSVToRGB(hue,1,1);
        proximityGUI.transform.RotateAround(proximityGUI.transform.position,Vector3.forward,(1-percentage)*2);
    }
    public void ShowProximity (bool isActive){
        isProximityOn = isActive;
        if(!isProximityOn)
            proximityGUI.gameObject.SetActive(false);
        else
            proximityGUI.gameObject.SetActive(true);
    }
}
