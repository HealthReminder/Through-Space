using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{   
    public bool isProximityOn = false;
    public bool isOrbitOn = false;
    public GameObject viewContainer;
    //public Image proximityGUI;
    public LineRenderer lineClosest, lineOrbiting, lineDirection;
    [Header("Spaceship")]
    public SpriteRenderer spaceshipSprite;

    [Header("Distance from star")]
    public Text TdistanceToStar;
    public GameObject TdistanceToStarObject;
    [Header("Death")]
    public ParticleSystem deathparticle;

      
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

    public void ToggleGUIContainer(int toggle){
        if(toggle == 1){
            viewContainer.SetActive(true);
        } else {
            viewContainer.SetActive(false);
        }
    }
    public void OnDeath(){
        deathparticle.gameObject.SetActive(true);
        spaceshipSprite.color = Color.black;
		deathparticle.Play(true);
        viewContainer.SetActive(false);
        ShowProximity(false);
    }
    public void ToggleDistanceFromStar(int toggle){
        if(toggle == 0){
            if(TdistanceToStarObject.activeSelf == true)
				TdistanceToStarObject.SetActive(false);
        }else {
            if(TdistanceToStarObject.activeSelf == false)
				TdistanceToStarObject.SetActive(true);
        }
    }

    public void ChangeDistanceText(string newText){
        TdistanceToStar.text = newText;
    }

    public void UpdateProximity (float percentage, Vector3 closestPlanetPosition){
        //Change the color of the GUI indicator
        //float hue = Mathf.Lerp(-0.065f,0.45f,1-percentage);
        //Debug.Log(percentage);
        //proximityGUI.color = Color.HSVToRGB(hue,1,1);
        //proximityGUI.transform.RotateAround(proximityGUI.transform.position,Vector3.forward,(1-percentage)*2);

        //Work the line that indicates the closes planet
        lineClosest.SetPosition(0,transform.position);
		lineClosest.SetPosition(1, closestPlanetPosition);
		lineClosest.colorGradient = new Gradient();
        lineClosest.startColor = new Color(1,1,1,1-percentage);
		lineClosest.endColor = new Color(1,1,1,1-percentage);
		lineClosest.enabled = true;
        
    }
    public void ShowProximity (bool isActive){
        return;
        if(isProximityOn == isActive)
            return;
        isProximityOn = isActive;
        //if(!isProximityOn)
            //proximityGUI.gameObject.SetActive(false);
        //else
            //proximityGUI.gameObject.SetActive(true);
    }

      public void ShowOrbit (bool isActive){
        if(isOrbitOn == isActive)
            return;
        isOrbitOn = isActive;
        if(!isOrbitOn)
            lineOrbiting.gameObject.SetActive(false);
        else
            lineOrbiting.gameObject.SetActive(true);
    }

    public void ToggleOrbit (float percentage, Vector3 closestPlanetPosition){
            //Change the color of the GUI indicator
            float hue = Mathf.Lerp(-0.065f,0.45f,1-percentage);
            //Debug.Log(percentage);
            lineOrbiting.startColor = Color.HSVToRGB(hue,1,1);
            lineOrbiting.endColor = Color.HSVToRGB(hue,1,1);

			//Set line positions
			lineOrbiting.SetPosition(0,transform.position);
			lineOrbiting.SetPosition(1, closestPlanetPosition);
            // float H, S, V;
            //Color.RGBToHSV(new Color(percentage, 1 - percentage, 0, 1), out H, out S, out V);
            //proximityGUI.color = Color.HSVToRGB(hue,1,1);

			//lineOrbiting.startColor = new Color(percentage,1-percentage,0,1);
			//lineOrbiting.endColor = new Color(percentage,1-percentage,0,1);
    }
}
