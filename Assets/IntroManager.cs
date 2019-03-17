using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public Text titleText;
    public Image titleBackground;
    public Image titleTopground;
    //public Animator rocketAnimator;
    void Start()
    {
        StartCoroutine(StartIntro());   
    }

   IEnumerator StartIntro(){

       //rocketAnimator.SetBool("On", true);.
       float waitTime = 4;
       bool skipped = false;

       //WAIT FOR ROCKET TO PASS
       while(waitTime > 0 && !skipped){
           Debug.Log("Intro rocket pass. " + waitTime);
           if(Input.touchCount>0|| Input.GetMouseButton(0))   
                skipped = true;
           yield return new WaitForSeconds(0.05f);
           waitTime -= 0.1f;
       }


        //SHOW TEXT AND BG
       while(titleText.color.a <1 && !skipped){
           Debug.Log("Intro show title.");
           if(Input.touchCount>0|| Input.GetMouseButton(0))   
                skipped = true;
           titleText.color+= new Color(0,0,0,0.02f);
           
           if(titleBackground.color.a <= 0.25f)
                titleBackground.color+= new Color(0,0,0,0.01f);

            yield return new WaitForSeconds(0.05f);
       }


       //LOAD SCENE
       waitTime = 2;
        AsyncOperation asyncLoadLevel;
        asyncLoadLevel = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        asyncLoadLevel.allowSceneActivation = false;


        //WAIT
       while(waitTime > 0&& !skipped){
           if(Input.touchCount>0|| Input.GetMouseButton(0))   
                skipped = true;

           yield return new WaitForSeconds(0.05f);
           waitTime-= 0.1f;
       }

        //FADE OUT
       while(titleTopground.color.a <1){
           //if(Input.touchCount>0 || Input.GetMouseButton(0))   
               // break;
           titleTopground.color+= new Color(0,0,0,0.03f);
           
            yield return new WaitForSeconds(0.05f);
       }

       //while (!asyncLoadLevel.isDone)
           //  yield return null;
            
         asyncLoadLevel.allowSceneActivation = true;


       yield break;
   }
}
