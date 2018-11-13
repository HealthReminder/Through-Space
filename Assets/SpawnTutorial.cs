using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTutorial : MonoBehaviour {

    public SpawnBehaviour spawn;
     TextWriter tw;
	void Start () {
        tw = GetComponent<TextWriter>();
        spawn.canSpawn = false;
        StartCoroutine(TutorialSequence());
	}
	
	// Update is called once per frame
	IEnumerator TutorialSequence () {

        tw.WriteText("Use the line below to visualize your route.", 0.075f);
        while (tw.writing == true)
            yield return null;
        FindObjectOfType<AudioManager>().Play("beepCommon");
        yield return new WaitForSeconds(1);
        tw.Clear();
        tw.WriteText("When you are happy with it use the left mouse button to be launched in that direction.", 0.075f);
        while (tw.writing == true)
            yield return null;
        yield return new WaitForSeconds(1);
        tw.Clear();
        spawn.canSpawn = true;
        while (!Input.GetMouseButtonDown(0))
            yield return null;
        FindObjectOfType<AudioManager>().Play("beepSuccess");

        yield return null;

        yield break;
	}
}
