using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonBehaviour : MonoBehaviour
{
    [HideInInspector] public int levelIndex;
    Button button;
    public Image image_star;
    Text text_name;
    Image button_bg;
    void Start()
    {
        button = GetComponent<Button>();
        button_bg = transform.GetChild(0).GetComponent<Image>();
        text_name = transform.GetChild(1).GetComponent<Text>();
        image_star = GetComponent<Image>();

    }
    public void Activate() {
        MenuManager.instance.SetGoToLevel(levelIndex);
    }

    public void UpdateView(StarController star){
        text_name.text = star.name;
        image_star.color = star.mainColor;
        text_name.color = star.detailColor;
        button_bg.color = star.mainColor + new Color(0,0,0,-0.8f);

        text_name.enabled = true;
        image_star.enabled = true;
        button.enabled = true;
        button_bg.enabled = true;
    }
    public void DisableView(){
        text_name.enabled = false;
        image_star.enabled = false;
        button.enabled = false;
        button_bg.enabled = false;
    }
}
