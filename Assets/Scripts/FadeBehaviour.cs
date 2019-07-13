using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBehaviour : MonoBehaviour {

	public float initialDelay = 1;
	float currentDelay;
	
	public Image[] images;

		//Play intro ost
        
	
	bool hasStarted = false;
	bool hasStartedFading = false;
	private void Update() {
		currentDelay += Time.deltaTime;
		if(!hasStarted){
			SoundtrackManager.instance.ChangeSet("Intro");
			StartCoroutine (FadeOut ());
			hasStarted = true;
		}else if(currentDelay >= initialDelay)
			if(Input.anyKeyDown||Input.touchCount > 0)
				if(hasStartedFading){
					hasStartedFading = false;
					StopCoroutine(FadeOut());
					for (int i = 0; i < images.Length; i++)
					images[i].gameObject.SetActive(false);
					//Play menu soundtrack
					SoundtrackManager.instance.ChangeSet("Menu");
				}


	}

	IEnumerator FadeOut() {
		Debug.Log(".");
		yield return null;
		if(images.Length <= 0)
			yield break;
		for (int i = 0; i < images.Length; i++)
			images[i].gameObject.SetActive(true);
		Debug.Log(".");
		yield return new WaitForSeconds(0.1f);

		hasStartedFading = true;
		Debug.Log(".");
		yield return new WaitForSeconds(2.1f);
			
		while (images[0].color.a > 0) {
			for (int i = 0; i < images.Length; i++)
			{
				images[i].color += new Color (0, 0, 0, -0.05f);
			}
			yield return new WaitForSeconds(Time.deltaTime*2);
		}
		Debug.Log(".");
		for (int i = 0; i < images.Length; i++)
			images[i].gameObject.SetActive(false);
		//Play menu soundtrack
		yield return null;
		if(hasStartedFading)
        	SoundtrackManager.instance.ChangeSet("Menu");
		hasStartedFading = false;
		yield break;
	}
}
