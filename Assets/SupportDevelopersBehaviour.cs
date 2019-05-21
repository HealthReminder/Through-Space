using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportDevelopersBehaviour : MonoBehaviour
{
    public bool isOn = false;
    public Transform container;
    private void Start() {
        ToggleSupportGUI(true);
    }
    private void Update() {
        if(isOn)
            if(Input.anyKeyDown || Input.GetMouseButton(0))
                ToggleSupportGUI(false);
    }

    private void ToggleSupportGUI(bool switchTo) {
        isOn = switchTo;
        StartCoroutine(WorkGUI(isOn));
    }

    IEnumerator WorkGUI(bool switchTo){
        if(switchTo){
            container.gameObject.SetActive(true);
            while(isOn){
                if(container.localPosition.y > 0)
                    container.localPosition += new Vector3(0,-10,0);
                yield return null;
            }
        }else
        while(!isOn){
            if(container.localPosition.y > -1000)
                container.localPosition += new Vector3(0,-10,0);
            else 
                container.gameObject.SetActive(false);
            yield return null;
        }
        yield break;
    }
}
