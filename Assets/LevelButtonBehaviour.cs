using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonBehaviour : MonoBehaviour
{
    [HideInInspector] public int levelIndex;
    public Button thisButton;
    public Image starImage;
    public Text nameText;
    public void Activate() {
        MenuManager.instance.SetGoToLevel(levelIndex);
    }

    public void UpdateView(StarController star){
        nameText.text = star.name;
        starImage.color = star.mainColor;
        nameText.color = star.detailColor;

        nameText.enabled = true;
        starImage.enabled = true;
        thisButton.enabled = true;
    }
    public void DisableView(){
        nameText.enabled = false;
        starImage.enabled = false;
        thisButton.enabled = false;
    }
}
