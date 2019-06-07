using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterView : MonoBehaviour
{
    public GameObject container;
    public Image titleBackground;
    public Text titleText;
    public Image explosionImage;
    public Transform explosionContainer;
    public AnimationCurve explosionCurve;
    public static ChapterView instance;


    
    void Update()
    {
        if(Input.anyKeyDown || Input.touchCount > 0){
            StopCoroutine(WorkTitle("", new Color(0,0,0,0),new Color(0,0,0,0)));
            container.SetActive(false);
        }
    }

    public void ShowTitle(string title, Color mainColor, Color detailColor){
        StartCoroutine(WorkTitle(title,mainColor,detailColor));
    }

    bool isWorking = false;
    float explosionProgress = 0;
    float titleDelay;
    int currentPhase = 0;
    bool hasEnabledUI = false;
    IEnumerator WorkTitle(string title, Color mainColor,Color detailColor) {
        isWorking = false;
        
        if(isWorking == false)
        yield return Setup(title,mainColor,detailColor);

        yield return null;
        isWorking = true;

        while(isWorking) {
            if(currentPhase == 0) {
                currentPhase = 1;
            } else if(currentPhase == 1){
                if(explosionProgress < 1) {
                    explosionContainer.localScale = new Vector3(1,explosionCurve.Evaluate(explosionProgress),1);
                    if(explosionProgress > 0.3f){
                        explosionImage.color+= new Color(0,0,0,Time.deltaTime*2);
                        if(!hasEnabledUI){
                            hasEnabledUI = true;
                            titleText.enabled = true;
                            titleBackground.color = new Color(0,0,0,0.8f);
                        }
                    }
                    if(explosionProgress <0.6f)
                    explosionProgress+= Time.deltaTime*2;
                    else
                    explosionProgress+= Time.deltaTime*1;
                   
                }else{
                    currentPhase = 2;
                }
            }  else if(currentPhase == 2){
                if(explosionImage.color.a > 0f)
                    explosionImage.color+= new Color(0,0,0,-Time.deltaTime*1);
                else{
                    currentPhase = 3;
                }
            } else if(currentPhase == 3){
                if(titleBackground.color.a > 0f){
                    titleBackground.color+= new Color(0,0,0,-Time.deltaTime*4);
                    titleText.color += new Color(0,0,0,-Time.deltaTime*4);
                } 
                if(titleText.color.a > 0f){
                    titleText.color += new Color(0,0,0,-Time.deltaTime*4);
                }else{
                    titleText.enabled = false;
                    currentPhase = 4;
                }
            }else if(currentPhase == 4){
                
                if(titleDelay >= 0) 
                    titleDelay -= Time.deltaTime*7;
                else {
                    currentPhase = 5;
                }
            } else {
                explosionContainer.localScale= new Vector3(1,0,1);
                container.SetActive(false);
                isWorking = false;
            }
            
            yield return null;
        }


        yield break;
    }

    IEnumerator Setup(string title, Color mainColor,Color detailColor) {
        container.SetActive(true);
        titleBackground.color+= new Color(0,0,0,-1f);
        titleText.enabled = false;
        explosionContainer.localScale = new Vector3(1,0,1);

        yield return null;

        titleText.text = title;
        explosionImage.color+= new Color(0,0,0,1f);
        yield return null;
        titleText.color = detailColor;
        explosionImage.color = mainColor;
        titleText.color += new Color(0,0,0,1f);
        yield return null;
        explosionProgress = 0;
        titleDelay = 1f;
        currentPhase = 0;
        hasEnabledUI = false;
        yield return null;
    }
    
}
