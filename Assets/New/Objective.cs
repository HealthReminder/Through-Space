using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Objective : MonoBehaviour {
	public string nextScene;
	void OnTriggerEnter2D (Collider2D coll) {
		if(coll.tag == "Player") {
			if (nextScene != null) {
                //PlayerPrefs.SetString("NextScene", nextScene);
                StartCoroutine(FadeOut());
            }
		}	
	}

    IEnumerator FadeOut()
    {
//        Image fader = FindObjectOfType<FadeOutOutro>().GetComponent<Image>();
      // while(fader.color.a < 1) { fader.color += new Color(0, 0, 0, 0.01f);yield return null; }
       // SceneManager.LoadScene("Trip");
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield break;
    }
}
