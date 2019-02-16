using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{   
    public bool isProximityOn = false;
    public bool isOrbitOn = false;
    public Image proximityGUI;
    public LineRenderer closestPlanetLine, orbitingPlanetLine;

      
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

    public void ToggleProximity (float percentage, Vector3 closestPlanetPosition){
        //Change the color of the GUI indicator
        float hue = Mathf.Lerp(-0.065f,0.45f,1-percentage);
        //Debug.Log(percentage);
        proximityGUI.color = Color.HSVToRGB(hue,1,1);
        proximityGUI.transform.RotateAround(proximityGUI.transform.position,Vector3.forward,(1-percentage)*2);

        //Work the line that indicates the closes planet
        closestPlanetLine.SetPosition(0,transform.position);
		closestPlanetLine.SetPosition(1, closestPlanetPosition);
		closestPlanetLine.colorGradient = new Gradient();
        closestPlanetLine.startColor = new Color(1,1,1,1-percentage);
		closestPlanetLine.endColor = new Color(1,1,1,1-percentage);
		closestPlanetLine.enabled = true;
        
    }
    public void ShowProximity (bool isActive){
        if(isProximityOn == isActive)
            return;
        isProximityOn = isActive;
        if(!isProximityOn)
            proximityGUI.gameObject.SetActive(false);
        else
            proximityGUI.gameObject.SetActive(true);
    }

      public void ShowOrbit (bool isActive){
        if(isOrbitOn == isActive)
            return;
        isOrbitOn = isActive;
        if(!isOrbitOn)
            orbitingPlanetLine.gameObject.SetActive(false);
        else
            orbitingPlanetLine.gameObject.SetActive(true);
    }

    public void ToggleOrbit (float percentage, Vector3 closestPlanetPosition){
            //Change the color of the GUI indicator
            float hue = Mathf.Lerp(-0.065f,0.45f,1-percentage);
            //Debug.Log(percentage);
            orbitingPlanetLine.startColor = Color.HSVToRGB(hue,1,1);
            orbitingPlanetLine.endColor = Color.HSVToRGB(hue,1,1);

			//Set line positions
			orbitingPlanetLine.SetPosition(0,transform.position);
			orbitingPlanetLine.SetPosition(1, closestPlanetPosition);
            // float H, S, V;
            //Color.RGBToHSV(new Color(percentage, 1 - percentage, 0, 1), out H, out S, out V);
            proximityGUI.color = Color.HSVToRGB(hue,1,1);

			//orbitingPlanetLine.startColor = new Color(percentage,1-percentage,0,1);
			//orbitingPlanetLine.endColor = new Color(percentage,1-percentage,0,1);
    }
}
