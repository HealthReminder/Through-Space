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

    public string testTitle; public Color testColor;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
            ShowTitle(testTitle, testColor);
    }

    void ShowTitle(string title, Color color){
        StartCoroutine(WorkTitle(title,color));
    }

    bool isWorking = false;

    float explosionProgress = 0;
    float titleDelay = 2;
    int currentPhase = 0;
    IEnumerator WorkTitle(string title, Color color) {
        isWorking = false;
        
        if(isWorking == false)
        yield return Setup(title,color);

        yield return null;
        isWorking = true;

        while(isWorking) {
            if(currentPhase == 0) {
                currentPhase = 1;
            } else if(currentPhase == 1){
                if(explosionProgress < 1) {
                    explosionContainer.localScale = new Vector3(1,explosionCurve.Evaluate(explosionProgress),1);
                    explosionProgress+= Time.deltaTime*3;
                }else{
                    titleText.enabled = true;
                    titleBackground.color = new Color(0,0,0,1f);
                    currentPhase = 2;
                }
            }  else if(currentPhase == 2){
                if(explosionImage.color.a > 0f)
                    explosionImage.color+= new Color(0,0,0,-0.02f);
                else{
                    currentPhase = 3;
                }
            } else if(currentPhase == 3){
                if(titleDelay >= 2) 
                    titleDelay -= Time.deltaTime;
                else {
                    currentPhase = 4;
                }
            }else if(currentPhase == 4){
                if(titleBackground.color.a > 0f){
                    titleBackground.color+= new Color(0,0,0,-0.02f);
                    titleText.color += new Color(0,0,0,-0.02f);
                } 
                if(titleText.color.a > 0f){
                    titleText.color += new Color(0,0,0,-0.02f);
                }else{
                    titleText.enabled = false;
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

    IEnumerator Setup(string title, Color color) {
        container.SetActive(true);
        titleBackground.color+= new Color(0,0,0,-1f);
        titleText.enabled = false;
        explosionContainer.localScale = new Vector3(1,0,1);

        yield return null;

        titleText.text = title;
        explosionImage.color+= new Color(0,0,0,1f);
        yield return null;
        titleText.color = color;
        explosionImage.color = color;
        titleText.color += new Color(0,0,0,1f);
        yield return null;
        explosionProgress = 0;
        titleDelay = 0;
        currentPhase = 0;
        yield return null;
    }
}
