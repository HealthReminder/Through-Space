using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBehaviour : MonoBehaviour {

	public float initialDelay = 1;
	public Image[] images;

	void Start () {
		//Play intro ost
        SoundtrackManager.instance.ChangeSet("Intro");
		StartCoroutine (FadeOut ());
	}
	IEnumerator FadeOut() {
		if(images.Length <= 0)
			yield break;
		for (int i = 0; i < images.Length; i++)
			images[i].gameObject.SetActive(true);

		yield return new WaitForSeconds(initialDelay);
			
		while (images[0].color.a > 0) {
			for (int i = 0; i < images.Length; i++)
			{
				images[i].color += new Color (0, 0, 0, -0.05f);
			}
			yield return new WaitForSeconds(Time.deltaTime*2);
		}
		
		for (int i = 0; i < images.Length; i++)
			images[i].gameObject.SetActive(false);
		//Play menu soundtrack
        SoundtrackManager.instance.ChangeSet("Menu");
	}
}
