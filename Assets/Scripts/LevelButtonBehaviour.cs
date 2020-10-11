using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonBehaviour : MonoBehaviour
{
    [HideInInspector] public int levelIndex;
    Button button;
    public Image image_star;
    public Text text_name;
    public Image button_bg;
    void Start()
    {
        button = GetComponent<Button>();

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
